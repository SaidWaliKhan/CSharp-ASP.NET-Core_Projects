namespace DocumentTextExtractionSystem.Api.Models;

public static class DocumentTypeResolver
{
    public static DocumentType Resolve(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return extension switch
        {
            ".txt" => DocumentType.PlainText,
            ".docx" => DocumentType.Word,
            ".xlsx" => DocumentType.Excel,
            ".pdf" => DocumentType.Pdf,
            ".png" or ".jpg" or ".jpeg" => DocumentType.Image,
            _ => DocumentType.Unsupported
        };
    }
}