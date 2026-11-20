namespace SocialCommentsApi.Middleware
{
    public class ApiKeyMiddleware
    {
        private const string HeaderName = "X-Api-Key";

        private readonly RequestDelegate _next;
        private readonly string _apiKey;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _apiKey = configuration["ApiKey"] ?? string.Empty;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                if (!context.Request.Headers.TryGetValue(HeaderName, out var provided)
                    || !string.Equals(provided, _apiKey, StringComparison.Ordinal))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("API key invalida o ausente.");
                    return;
                }
            }

            await _next(context);
        }
    }
}
