using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using ItHappened.Application.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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
            object response = null;

            switch (ex)
            {
                case BusinessException businessException:
                    logger.LogError(ex, "Business exception");
                    response = new ExceptionResponse(businessException.ErrorMessage, businessException.Payload);
                    context.Response.StatusCode = (int)businessException.HttpErrorCode;
                    break;
                case Exception e:
                    logger.LogError(ex, "Application exception");
                    response = new ExceptionResponse(ex.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            if (response != null)
            {
                var result = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(result);
            }
        }
    }
}