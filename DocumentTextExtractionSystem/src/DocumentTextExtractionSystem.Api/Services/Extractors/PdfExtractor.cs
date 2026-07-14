using System.Diagnostics;
using System.Text;
using DocumentTextExtractionSystem.Api.Interfaces;
using DocumentTextExtractionSystem.Api.Models;
using UglyToad.PdfPig;

namespace DocumentTextExtractionSystem.Api.Services.Extractors;

public class PdfExtractor : IDocumentTextExtractor
{
    private const int MinCharsToConsiderPageTextBased = 10;

    private readonly IOcrService _ocrService;
    private readonly ILogger<PdfExtractor> _logger;

    public PdfExtractor(IOcrService ocrService, ILogger<PdfExtractor> logger)
    {
        _ocrService = ocrService;
        _logger = logger;
    }

    public DocumentType SupportedType => DocumentType.Pdf;

    public async Task<ExtractionResult> ExtractTextAsync(Stream fileStream, string fileName)
    {
    
        var tempPdfPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");

        try
        {
            await using (var fileOnDisk = File.Create(tempPdfPath))
            {
                await fileStream.CopyToAsync(fileOnDisk);
            }

            var builder = new StringBuilder();
            var anyPageUsedOcr = false;

            using (var document = PdfDocument.Open(tempPdfPath))
            {
                foreach (var page in document.GetPages())
                {
                    var pageText = page.Text;

                    if (!string.IsNullOrWhiteSpace(pageText) && pageText.Length >= MinCharsToConsiderPageTextBased)
                    {
                        // Text-based page — cheap, direct extraction, no OCR needed.
                        builder.AppendLine(pageText);
                    }
                    else
                    {
                        // Likely a scanned/image page — rasterize and OCR it.
                        _logger.LogInformation(
                            "Page {PageNumber} of {FileName} has no text layer — falling back to OCR.",
                            page.Number, fileName);

                        var ocrText = await RasterizeAndOcrPageAsync(tempPdfPath, page.Number);
                        builder.AppendLine(ocrText);
                        anyPageUsedOcr = true;
                    }
                }
            }

            var text = builder.ToString();

            if (string.IsNullOrWhiteSpace(text))
            {
                return ExtractionResult.Fail("No extractable text found in the PDF.", SupportedType, fileName);
            }

            return ExtractionResult.Ok(text, SupportedType, fileName, usedOcr: anyPageUsedOcr);
        }
        catch (Exception ex)
        {
            return ExtractionResult.Fail($"Failed to process PDF: {ex.Message}", SupportedType, fileName);
        }
        finally
        {
            // Always clean up the temp file, even if something above threw.
            if (File.Exists(tempPdfPath))
            {
                File.Delete(tempPdfPath);
            }
        }
    }

    /// <summary>
    /// Renders a single PDF page to a PNG using the `pdftoppm` CLI tool
    /// (from the poppler-utils apt package), then runs OCR on that image.
    /// </summary>
    private async Task<string> RasterizeAndOcrPageAsync(string pdfPath, int pageNumber)
    {
        var outputPrefix = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var expectedPngPath = $"{outputPrefix}-{pageNumber:D2}.png";

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "pdftoppm",
                Arguments = $"-png -f {pageNumber} -l {pageNumber} -r 300 \"{pdfPath}\" \"{outputPrefix}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo)
                ?? throw new InvalidOperationException("Failed to start pdftoppm process.");

            var stderr = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"pdftoppm exited with code {process.ExitCode}: {stderr}");
            }

            if (!File.Exists(expectedPngPath))
            {
                throw new InvalidOperationException(
                    $"pdftoppm did not produce the expected output file '{expectedPngPath}'.");
            }

            var imageBytes = await File.ReadAllBytesAsync(expectedPngPath);
            return await _ocrService.RecognizeTextAsync(imageBytes);
        }
        finally
        {
            if (File.Exists(expectedPngPath))
            {
                File.Delete(expectedPngPath);
            }
        }
    }
}