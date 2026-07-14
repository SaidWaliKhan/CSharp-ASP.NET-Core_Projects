using DocumentTextExtractionSystem.Api.Models;

namespace DocumentTextExtractionSystem.Api.Interfaces;

public interface IExtractionPipeline
{
    Task<ExtractionResult> ProcessAsync(Stream fileStream, string fileName);
}