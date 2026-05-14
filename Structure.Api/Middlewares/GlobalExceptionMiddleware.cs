using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using Structure.Data.Common;

namespace Structure.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
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
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation exception occurred");

            var errors = ex.Errors
                .Select(x => x.ErrorMessage)
                .Distinct()
                .ToList();

            await HandleExceptionAsync(
                context,
                HttpStatusCode.BadRequest,
                "Validation failed",
                errors);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update exception occurred");

            await HandleExceptionAsync(
                context,
                HttpStatusCode.InternalServerError,
                "Database operation failed",
                new List<string>
                {
                    ex.InnerException?.Message ?? ex.Message
                });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized access");

            await HandleExceptionAsync(
                context,
                HttpStatusCode.Unauthorized,
                "Unauthorized access",
                new List<string>
                {
                    ex.Message
                });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found");

            await HandleExceptionAsync(
                context,
                HttpStatusCode.NotFound,
                "Resource not found",
                new List<string>
                {
                    ex.Message
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            await HandleExceptionAsync(
                context,
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred",
                new List<string>
                {
                    ex.Message
                });
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string message,
        List<string> errors)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ApiResponse<object>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors,
            Data = null
        };

        var jsonResponse = JsonSerializer.Serialize(
            response,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

        await context.Response.WriteAsync(jsonResponse);
    }
}