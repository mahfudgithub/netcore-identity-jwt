using LoggerService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace hidayah_collage.Models.Exceptions
{
    public class GlobalExceptionErrorHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _loggerManager;

        public GlobalExceptionErrorHandling(RequestDelegate next, ILoggerManager loggerManager )
        {
            _next = next;
            _loggerManager = loggerManager;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var exceptionType = exception.GetType();
            string message = String.Empty;
            message = exception.Message;
            context.Response.ContentType = "application/json";
            if (exceptionType == typeof(NotFoundException)) 
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else if(exceptionType == typeof(BadRequestException))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(InvalidException))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            }.ToString().ToLower());
            /*
            HttpStatusCode status = HttpStatusCode.OK;
            var stackTrace = String.Empty;
            string message = String.Empty; ;
            var exceptionType = exception.GetType();

            if (exceptionType == typeof(NotFoundException))
            {
                message = exception.Message;
                status = HttpStatusCode.NotFound;
                stackTrace = exception.StackTrace;
            }
            var exceptionResult = JsonSerializer.Serialize(new
            {
                error = message,
                stackTrace
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(exceptionResult);
            */
        }
    }
}
