using Serilog;


namespace LogReg_Identity.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unhandled exception occurred while processing the request.");
                throw; // Re-throw the exception to ensure it's still handled by the exception handling middleware
            }
        }
    }

}
