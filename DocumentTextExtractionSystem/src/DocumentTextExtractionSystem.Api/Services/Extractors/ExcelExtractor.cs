using System.Text;
using ClosedXML.Excel;
using DocumentTextExtractionSystem.Api.Interfaces;
using DocumentTextExtractionSystem.Api.Models;

namespace DocumentTextExtractionSystem.Api.Services.Extractors;

public class ExcelExtractor : IDocumentTextExtractor
{
    public DocumentType SupportedType => DocumentType.Excel;

    public async Task<ExtractionResult> ExtractTextAsync(Stream fileStream, string fileName)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            using var workbook = new XLWorkbook(memoryStream);
            var builder = new StringBuilder();

            foreach (var worksheet in workbook.Worksheets)
            {
                var usedRange = worksheet.RangeUsed();
                if (usedRange is null)
                {
                    continue; // empty sheet, nothing to extract
                }

                builder.AppendLine($"--- Sheet: {worksheet.Name} ---");

                foreach (var row in usedRange.RowsUsed())
                {
                    var cellValues = row.Cells()
                        .Select(cell => cell.GetFormattedString());

                    builder.AppendLine(string.Join('\t', cellValues));
                }
            }

            var text = builder.ToString();

            if (string.IsNullOrWhiteSpace(text))
            {
                return ExtractionResult.Fail("No content found in the Excel file (all sheets are empty).", SupportedType, fileName);
            }

            return ExtractionResult.Ok(text, SupportedType, fileName);
        }
        catch (Exception ex)
        {
            return ExtractionResult.Fail($"Failed to read Excel file: {ex.Message}", SupportedType, fileName);
        }
    }
}