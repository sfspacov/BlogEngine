﻿using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BlogEngine.Shared.Models;
using Microsoft.AspNetCore.Http;

namespace BlogEngine.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorDetails = new ErrorDetails(httpContext.Response.StatusCode, $"Internal Server Error: { ex.Message }");

            string errorDetailsJson = JsonSerializer.Serialize(errorDetails);

            await httpContext.Response.WriteAsync(errorDetailsJson);
        }
    }
}