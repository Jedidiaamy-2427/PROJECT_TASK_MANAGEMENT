using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagerAPI.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidOperationException ex)
            {
                await WriteProblemDetails(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await WriteProblemDetails(context, HttpStatusCode.InternalServerError, "Une erreur est survenue.");
            }
        }

        private static async Task WriteProblemDetails(HttpContext context, HttpStatusCode status, string message)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)status;
            var problem = new ProblemDetails
            {
                Status = (int)status,
                Title = Enum.GetName(typeof(HttpStatusCode), status),
                Detail = message,
                Instance = context.Request.Path
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}


