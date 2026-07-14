using DocumentTextExtractionSystem.Api.Interfaces;
using Tesseract;

namespace DocumentTextExtractionSystem.Api.Services;

public class OcrService : IOcrService
{
    private readonly string _tessDataPath;
    private readonly ILogger<OcrService> _logger;

    public OcrService(ILogger<OcrService> logger)
    {
        _logger = logger;

        _tessDataPath = Path.Combine(AppContext.BaseDirectory, "tessdata");

        if (!Directory.Exists(_tessDataPath) || !File.Exists(Path.Combine(_tessDataPath, "eng.traineddata")))
        {
            throw new InvalidOperationException(
                $"Tesseract language data not found at '{_tessDataPath}'. " +
                "Ensure eng.traineddata exists in the project's tessdata/ folder " +
                "and that the .csproj copies it to the output directory.");
        }
    }

    public Task<string> RecognizeTextAsync(byte[] imageBytes)
    {
        return Task.Run(() =>
        {
            try
            {
                using var engine = new TesseractEngine(_tessDataPath, "eng", EngineMode.Default);
                using var img = Pix.LoadFromMemory(imageBytes);
                using var page = engine.Process(img);
                return page.GetText() ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OCR failed. Inner: {Inner}", ex.InnerException?.ToString() ?? "none");
                throw;
            }
        });
    }
}