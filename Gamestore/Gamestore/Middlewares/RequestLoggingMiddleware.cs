using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Gamestore.Middlewares;

[ExcludeFromCodeCoverage]
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var request = context.Request;

        request.EnableBuffering();
        var requestContent = await new StreamReader(request.Body).ReadToEndAsync();
        request.Body.Position = 0;

        await _next(context);

        stopwatch.Stop();

        var response = context.Response;

        var logEntry = $"""
        Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}
        IP Address: {context.Connection.RemoteIpAddress}
        URL: {request.Method} {request.Path}
        Status Code: {response.StatusCode}
        Request Content: {requestContent}
        Response Time: {stopwatch.ElapsedMilliseconds} ms

        """;

        var logPath = $"Logs/requests-{DateTime.UtcNow:yyyy-MM-dd}.txt";
        Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);
        await File.AppendAllTextAsync(logPath, logEntry);

        _logger.LogInformation(logEntry);
    }
}
