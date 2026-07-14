using DocumentTextExtractionSystem.Api.Interfaces;
using DocumentTextExtractionSystem.Api.Services;
using DocumentTextExtractionSystem.Api.Services.Extractors;

namespace DocumentTextExtractionSystem.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDocumentExtractionServices(this IServiceCollection services)
    {
        services.AddScoped<IDocumentTextExtractor, PlainTextExtractor>();
        services.AddScoped<IDocumentTextExtractor, WordDocumentExtractor>();
        services.AddScoped<IDocumentTextExtractor, ExcelExtractor>();
        services.AddScoped<IDocumentTextExtractor, PdfExtractor>();
        services.AddScoped<IDocumentTextExtractor, ImageExtractor>();

        services.AddScoped<IOcrService, OcrService>();
        services.AddScoped<IExtractionPipeline, ExtractionPipeline>();

        return services;
    }
}