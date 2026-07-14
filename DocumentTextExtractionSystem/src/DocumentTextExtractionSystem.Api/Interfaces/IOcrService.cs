namespace DocumentTextExtractionSystem.Api.Interfaces;


/// Wraps the Tesseract OCR engine. Kept as its own interface (separate from
/// IDocumentTextExtractor) because OCR is a capability used by TWO different
/// extractors: ImageExtractor (always) and PdfExtractor (only for scanned PDFs).

public interface IOcrService
{

    Task<string> RecognizeTextAsync(byte[] imageBytes);
}