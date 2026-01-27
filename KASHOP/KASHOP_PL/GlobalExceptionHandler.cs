using KASHOP.DAL.DTO.Response;
using Microsoft.AspNetCore.Diagnostics;

namespace KASHOP.PL
{
    public class GlobalExceptionHandler: IExceptionHandler
    {

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            var errorDetails = new ErrorDetails()
            {
                StatusCode  = StatusCodes.Status500InternalServerError,
                Message = "Server Error",
                //StackTrace = e.InnerException.Message
            };
            context.Response.StatusCode= StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(errorDetails);
            return true;
        }
    }
}
