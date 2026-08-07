using System.Net;
using System.Text.Json;
using FluentValidation;

namespace SMS.WebApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

        var (statusCode, title, errors) = MapException(exception);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var problemDetails = new
        {
            status = statusCode,
            title,
            errors,
            traceId = context.TraceIdentifier,
            // ONLY in Development — never leak this in production, since it can contain
            // stack traces, file paths, sometimes even parts of connection strings.
            detail = _environment.IsDevelopment() ? exception.ToString() : null
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }

    private static (int StatusCode, string Title, object? Errors) MapException(Exception exception)
    {
        return exception switch
        {
            ValidationException validationEx => (
                (int)HttpStatusCode.BadRequest,
                "One or more validation errors occurred.",
                validationEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            ),

            InvalidOperationException businessRuleEx => (
                (int)HttpStatusCode.Conflict,
                businessRuleEx.Message,
                null
            ),

            KeyNotFoundException => (
                (int)HttpStatusCode.NotFound,
                "The requested resource was not found.",
                null
            ),

            _ => (
                (int)HttpStatusCode.InternalServerError,
                "An unexpected error occurred. Please try again later.",
                null
            )
        };
    }
}