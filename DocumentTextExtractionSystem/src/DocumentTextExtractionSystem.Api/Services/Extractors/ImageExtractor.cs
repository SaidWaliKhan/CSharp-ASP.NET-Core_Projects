using DocumentTextExtractionSystem.Api.Interfaces;
using DocumentTextExtractionSystem.Api.Models;

namespace DocumentTextExtractionSystem.Api.Services.Extractors;

public class ImageExtractor : IDocumentTextExtractor
{
    private readonly IOcrService _ocrService;

    public ImageExtractor(IOcrService ocrService)
    {
        _ocrService = ocrService;
    }

    public DocumentType SupportedType => DocumentType.Image;

    public async Task<ExtractionResult> ExtractTextAsync(Stream fileStream, string fileName)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();

            if (imageBytes.Length == 0)
            {
                return ExtractionResult.Fail("The uploaded image is empty.", SupportedType, fileName);
            }

            var text = await _ocrService.RecognizeTextAsync(imageBytes);

            if (string.IsNullOrWhiteSpace(text))
            {
                return ExtractionResult.Fail(
                    "OCR could not detect any text in the image.", SupportedType, fileName);
            }

            return ExtractionResult.Ok(text, SupportedType, fileName, usedOcr: true);
        }
        catch (Exception ex)
        {
            return ExtractionResult.Fail($"Failed to process image: {ex.Message}", SupportedType, fileName);
        }
    }
}