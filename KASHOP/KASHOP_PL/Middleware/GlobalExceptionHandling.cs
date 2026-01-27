using KASHOP.DAL.DTO.Response;
using Microsoft.AspNetCore.Http;

namespace KASHOP.PL.Middleware
{
    public class GlobalExceptionHandling
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandling(RequestDelegate next)
        {
            _next=next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                var errorDetails = new ErrorDetails()
                {
                    StatusCode  = StatusCodes.Status500InternalServerError,
                    Message = "Server Error",
                    StackTrace = e.InnerException.Message
                };
                context.Response.StatusCode= StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(errorDetails);
            }
        }
    }
}
