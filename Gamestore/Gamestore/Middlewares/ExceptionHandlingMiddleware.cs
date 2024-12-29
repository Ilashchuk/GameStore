using System.Diagnostics.CodeAnalysis;

namespace Gamestore.Middlewares;

[ExcludeFromCodeCoverage]
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var errorLog = $"""
        Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}
        Exception: {exception.GetType()}
        Message: {exception.Message}
        Stack Trace: {exception.StackTrace}

        """;

        var logPath = $"Logs/errors-{DateTime.UtcNow:yyyy-MM-dd}.txt";
        Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);
        await File.AppendAllTextAsync(logPath, errorLog);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "An unexpected error occurred. Please try again later.",
            Detail = exception.Message,
        };

        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }
}
