﻿using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace hidayah_collage.Models.Exceptions
{
    public static class ExceptionMiddlewareExtentions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionErrorHandling>();
            //app.UseExceptionHandler(appError =>
            //{
            //    appError.Run(async context =>
            //    {
            //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //        context.Response.ContentType = "application/json";
            //        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            //        if (contextFeature != null)
            //        {
            //            logger.LogError($"Something went wrong: {contextFeature.Error}");
            //            await context.Response.WriteAsync(new ErrorDetails()
            //            {
            //                StatusCode = context.Response.StatusCode,
            //                Message = "Internal Server Error."
            //            }.ToString());
            //        }
            //    });
            //});
        }
    }
}
