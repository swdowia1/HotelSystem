using System.Diagnostics;

namespace ReservationService
{
    public class ResponseTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseTimeMiddleware> _logger;

        public ResponseTimeMiddleware(RequestDelegate next, ILogger<ResponseTimeMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();

            _logger.LogInformation("Request [{method}] {url} took {time} ms",
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds);
        }
    }

}
