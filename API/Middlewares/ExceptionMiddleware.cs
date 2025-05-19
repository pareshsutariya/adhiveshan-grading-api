using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace AdhiveshanGrading.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            //context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var response = ServiceResponse.Fail($"{ex.Message} {ex.InnerException?.Message}");
            await context.Response.WriteAsJsonAsync(response);

            //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // if (ex is ApplicationException)
            // {
            //     await context.Response.WriteAsync(ex.Message);
            // }
        }
    }
}