using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex) // 👈 Manejar errores de validación
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
            var response = new { StatusCode = 400, Message = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (KeyNotFoundException ex) // 👈 Manejar errores de "No encontrado"
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound; // 404
            var response = new { StatusCode = 404, Message = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception ex) // 👈 Manejar errores generales
        {
            _logger.LogError(ex, "Ocurrió un error no manejado.");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
            var response = new { StatusCode = 500, Message = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        
    }

}
