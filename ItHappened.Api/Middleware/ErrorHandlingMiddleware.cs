using System;
using System.Net;
using System.Threading.Tasks;
using ItHappened.Application.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ItHappened.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
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
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ErrorHandlingMiddleware> logger)
        {
            var response = ex.Message;

            if (ex is BusinessException businessException)
            {
                logger.LogError(ex, "Business exception");
                response = JsonConvert.SerializeObject(businessException);
                context.Response.StatusCode = (int) businessException.HttpErrorCode;
            }
            else
            {
                logger.LogError(ex, "Unexpected exception");
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(response);
        }
    }
}