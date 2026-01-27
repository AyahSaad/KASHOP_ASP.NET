namespace KASHOP.PL.Middleware
{
    public class CustomeMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomeMiddleware(RequestDelegate next)
        {
            _next=next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("Processing Request");
            await _next(context);
            Console.WriteLine("Processing Response");
        }
    }
}
