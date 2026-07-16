using DocumentTextExtractionSystem.Api.Interfaces;
using DocumentTextExtractionSystem.Api.Models;

namespace DocumentTextExtractionSystem.Api.Services.Extractors;

// this just for plain .text files
public class PlainTextExtractor : IDocumentTextExtractor
{
    public DocumentType SupportedType => DocumentType.PlainText;

    public async Task<ExtractionResult> ExtractTextAsync(Stream Filestream, string fileName)
    {
        try

        {

            using var reader = new StreamReader(Filestream);
            var text = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(text))
            {
                return ExtractionResult.Fail("the text file is empty", SupportedType, fileName);
            }

            return ExtractionResult.Ok(text, SupportedType, fileName);
        } 
        catch(Exception ex)
        {
            return ExtractionResult.Fail($"Failed to read the text, {ex.Message}", SupportedType, fileName);
        }   
    }
}