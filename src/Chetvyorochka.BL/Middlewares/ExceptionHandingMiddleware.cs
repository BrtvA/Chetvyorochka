using Chetvyorochka.BL.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chetvyorochka.BL.Middlewares
{
    public class ExceptionHandingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandingMiddleware> _logger;

        public ExceptionHandingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    HttpStatusCode.NotFound,
                    ex.Message);
            }
            catch (BadRequestException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
            catch (DbUpdateException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    HttpStatusCode.InternalServerError,
                    "Такой объект уже есть");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    HttpStatusCode.InternalServerError,
                    "Непредвиденная ошибка");
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            string exMsg,
            HttpStatusCode httpStatusCode,
            string message)
        {
            _logger.LogError($"{DateTime.Now}: {exMsg}");

            HttpResponse response = context.Response;
            response.ContentType = "text/html";
            response.StatusCode = (int)httpStatusCode;

            await response.WriteAsJsonAsync(new { errorText = message });
        }
    }
}
