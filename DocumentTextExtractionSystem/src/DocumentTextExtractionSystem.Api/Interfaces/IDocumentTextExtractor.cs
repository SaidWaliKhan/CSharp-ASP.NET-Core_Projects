using DocumentTextExtractionSystem.Api.Models;

namespace DocumentTextExtractionSystem.Api.Interfaces;

public interface IDocumentTextExtractor
{
    DocumentType SupportedType { get; }

    Task<ExtractionResult> ExtractTextAsync(Stream fileStream, string fileName);
}