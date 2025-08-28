using System.Net;
using System.Text.Json;

namespace LotusAscend.Middleware;

/// <summary>
/// Catches unhandled exceptions in the request pipeline and returns a standardized JSON error response.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred.");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // ensure both anonymous types have the same properties.
            var response = _env.IsDevelopment()
                ? new { message = ex.Message, details = ex.StackTrace?.ToString() }
                : new { message = "An internal server error occurred.", details = (string?)null };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}