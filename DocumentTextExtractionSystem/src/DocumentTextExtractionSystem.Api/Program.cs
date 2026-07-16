using DocumentTextExtractionSystem.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// --- Service registration ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// All of our document extraction DI wiring lives in one call —
builder.Services.AddDocumentExtractionServices();

// we increase the MultiPartBody to upload large MB Files.
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 25 * 1024 * 1024; // 25 MB
});

var app = builder.Build();

// --- Middleware pipeline ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();