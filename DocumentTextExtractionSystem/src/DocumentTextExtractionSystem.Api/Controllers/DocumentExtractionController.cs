using DocumentTextExtractionSystem.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DocumentTextExtractionSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentExtractionController : ControllerBase
{
    private const long MaxFileSizeBytes = 25 * 1024 * 1024; // 25 MB

    private readonly IExtractionPipeline _pipeline;
    private readonly ILogger<DocumentExtractionController> _logger;

    public DocumentExtractionController(IExtractionPipeline pipeline, ILogger<DocumentExtractionController> logger)
    {
        _pipeline = pipeline;
        _logger = logger;
    }


    [HttpPost("extract")]
    [RequestSizeLimit(MaxFileSizeBytes)]
    public async Task<IActionResult> ExtractText(IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(new { error = "No file was uploaded, or the file is empty." });
        }

        if (file.Length > MaxFileSizeBytes)
        {
            return BadRequest(new { error = $"File exceeds the maximum allowed size of {MaxFileSizeBytes / (1024 * 1024)} MB." });
        }

        _logger.LogInformation("Received upload: {FileName} ({SizeKb} KB)", file.FileName, file.Length / 1024);

        await using var stream = file.OpenReadStream();
        var result = await _pipeline.ProcessAsync(stream, file.FileName);

        if (!result.Success)
        {
            return UnprocessableEntity(new
            {
                error = result.ErrorMessage,
                fileName = result.FileName,
                documentType = result.DocumentType.ToString()
            });
        }

        return Ok(new
        {
            fileName = result.FileName,
            documentType = result.DocumentType.ToString(),
            usedOcr = result.UsedOcr,
            characterCount = result.Text.Length,
            text = result.Text
        });
    }
}