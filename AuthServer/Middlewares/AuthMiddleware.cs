namespace AuthServer.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _loginPath;

        public AuthMiddleware(RequestDelegate next, string loginPath)
        {
            _next = next;
            _loginPath = loginPath;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization") && context.Request.Path == "/auth/authorize")
            {

                var originalPath = context.Request.Path + context.Request.QueryString;
                context.Items["OriginalPath"] = originalPath;

                // prebaci na login page
                var redirectUrl = $"{_loginPath}?returnUrl={Uri.EscapeDataString(originalPath)}";
                context.Response.Redirect(redirectUrl);
                return;
            }

            await _next(context);
        }
    }
}