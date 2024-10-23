using System.Net;
using System.Text.Json;
using MosEisleyCantina.MosEisleyCantina.Common.Models;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
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

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        switch (ex)
        {
            case ArgumentNullException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                break;
            case ArgumentException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                break;
            case InvalidOperationException:
                context.Response.StatusCode = (int)HttpStatusCode.Conflict; // 409
                break;
            case KeyNotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound; // 404
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
                break;
        }

        // Log the exception (you can use a logging framework here)
        Console.WriteLine($"Something went wrong: {ex.Message}");

        var result = JsonSerializer.Serialize(new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Message = ex.Message // Return the exception message (can be customized)
        });

        return context.Response.WriteAsync(result);
    }
}
