using DocumentTextExtractionSystem.Api.Interfaces;
using DocumentTextExtractionSystem.Api.Models;

namespace DocumentTextExtractionSystem.Api.Services;

public class ExtractionPipeline : IExtractionPipeline
{
    private readonly Dictionary<DocumentType, IDocumentTextExtractor> _extractorsByType;
    private readonly ILogger<ExtractionPipeline> _logger;

    public ExtractionPipeline(IEnumerable<IDocumentTextExtractor> extractors, ILogger<ExtractionPipeline> logger)
    {
        // Every IDocumentTextExtractor registered in DI (Part 2 & 3) arrives
        // here automatically as a collection. We index them by SupportedType
        // once, so routing is an O(1) dictionary lookup per request instead
        // of a linear scan (or worse, a hardcoded switch statement listing
        // every extractor class by name).
        _extractorsByType = extractors.ToDictionary(e => e.SupportedType);
        _logger = logger;
    }

    public async Task<ExtractionResult> ProcessAsync(Stream fileStream, string fileName)
    {
        var documentType = DocumentTypeResolver.Resolve(fileName);

        if (documentType == DocumentType.Unsupported)
        {
            _logger.LogWarning("Rejected unsupported file type for {FileName}", fileName);

            return ExtractionResult.Fail(
                $"Unsupported file type for '{fileName}'. Supported types: .txt, .docx, .xlsx, .pdf, .png, .jpg, .jpeg",
                DocumentType.Unsupported,
                fileName);
        }

        if (!_extractorsByType.TryGetValue(documentType, out var extractor))
        {
           
            _logger.LogError("No extractor registered for document type {DocumentType}", documentType);
            return ExtractionResult.Fail(
                $"No extractor is configured for document type '{documentType}'.", documentType, fileName);
        }

        _logger.LogInformation("Processing {FileName} as {DocumentType}", fileName, documentType);

        var result = await extractor.ExtractTextAsync(fileStream, fileName);

        if (result.Success)
        {
            _logger.LogInformation(
                "Successfully extracted {CharCount} characters from {FileName} (OCR used: {UsedOcr})",
                result.Text.Length, fileName, result.UsedOcr);
        }
        else
        {
            _logger.LogWarning("Extraction failed for {FileName}: {Error}", fileName, result.ErrorMessage);
        }

        return result;
    }
}