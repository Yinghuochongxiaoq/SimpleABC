using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SimpleABC.Api.Interface.ILogHistoryBll;

namespace SimpleABC.Api.Program.MiddleWares
{
    /// <summary>
    /// Error handler middleware
    /// </summary>
    public class ExceptionHandlerMiddleWare
    {
        /// <summary>
        /// next pipe.
        /// </summary>
        private readonly RequestDelegate _next;

        private readonly IErrorLogBll _errorLogBll;

        /// <summary>
        /// construct function
        /// </summary>
        /// <param name="next"></param>
        public ExceptionHandlerMiddleWare(RequestDelegate next, IErrorLogBll errorLogBll)
        {
            _next = next;
            _errorLogBll = errorLogBll;
        }

        /// <summary>
        /// 中间件管道
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
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

        /// <summary>
        /// deal error.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception == null) return;
            await WriteExceptionAsync(context, exception).ConfigureAwait(false);
        }

        private async Task WriteExceptionAsync(HttpContext context, Exception exception)
        {
            //记录错误
            await _errorLogBll.WriteErrorInfo(exception, context);
            //返回友好的提示
            var response = context.Response;

            //状态码
            if (exception is UnauthorizedAccessException)
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            else if (exception != null)
                response.StatusCode = (int)HttpStatusCode.BadRequest;

            response.ContentType = context.Request.Headers["Accept"];
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync("<html><head><title>Error</title></head><body>");
            await context.Response.WriteAsync($"<h3 style='color:red;'>{exception?.Message}</h3>");
            await context.Response.WriteAsync($"<p>Type: {exception?.GetType().FullName}");
            await context.Response.WriteAsync($"<p>StackTrace: {exception?.StackTrace}");
            await context.Response.WriteAsync("</body></html>");
        }

    }
}
