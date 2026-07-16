namespace DocumentTextExtractionSystem.Api.Models;

public class ExtractionResult
{
    public bool Success { get; set; }

    public string Text { get; set; } = string.Empty;

    public DocumentType DocumentType { get; set; }

    public string FileName { get; set; } = string.Empty;

    public bool UsedOcr { get; set; }

    public string? ErrorMessage { get; set; }

    public static ExtractionResult Ok(string text, DocumentType type, string fileName, bool usedOcr = false)
        => new()
        {
            Success = true,
            Text = text,
            DocumentType = type,
            FileName = fileName,
            UsedOcr = usedOcr
        };

    public static ExtractionResult Fail(string errorMessage, DocumentType type, string fileName)
        => new()
        {
            Success = false,
            ErrorMessage = errorMessage,
            DocumentType = type,
            FileName = fileName
        };
}