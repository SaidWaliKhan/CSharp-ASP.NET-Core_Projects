
using System.Text;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentTextExtractionSystem.Api.Interfaces;
using DocumentTextExtractionSystem.Api.Models;

namespace DocumentTextExtractionSystem.Api.Services.Extractors;

public class WordDocumentExtractor : IDocumentTextExtractor
{
    public DocumentType SupportedType => DocumentType.Word;

    public async Task<ExtractionResult> ExtractTextAsync( Stream fileStram , string fileName)
    {
        try

        {
            using var stream = new MemoryStream();
            await fileStram.CopyToAsync(stream);

            stream.Position = 0;


            using var wordDocument = WordprocessingDocument.Open(stream, false);

            var body = wordDocument.MainDocumentPart?.Document?.Body;

            if (body is null)
            {
                return ExtractionResult.Fail("The Word document has no readable body content.", SupportedType, fileName);
            }

            var builder = new StringBuilder();
            foreach (var paragraph in body.Elements<Paragraph>())
            {
                builder.AppendLine(paragraph.InnerText);
            }

            var text = builder.ToString();

            if (string.IsNullOrWhiteSpace(text))
            {
                return ExtractionResult.Fail("No text content found in the Word document.", SupportedType, fileName);
            }

            return ExtractionResult.Ok(text, SupportedType, fileName);


        }
        catch (Exception ex)
        {
            return ExtractionResult.Fail($"Failed to read Word document: {ex.Message}", SupportedType, fileName);
        }
    }

   
}
    

    