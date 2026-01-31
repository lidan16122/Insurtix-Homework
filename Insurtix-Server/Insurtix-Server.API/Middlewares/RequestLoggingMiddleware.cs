using System.Text;

namespace Insurtix_Server.API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log request method, path, and query string
            Console.WriteLine($"[Request] {context.Request.Method} {context.Request.Path}{context.Request.QueryString}");

            // Log request body for POST/PUT
            if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put)
            {
                context.Request.EnableBuffering(); // allows us to read the body without consuming it
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                string body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; // reset for next middleware
                if (!string.IsNullOrWhiteSpace(body))
                {
                    Console.WriteLine($"[Request Body] {body}");
                }
            }

            // Call the next middleware/controller
            await _next(context);

            // Log the response status code
            Console.WriteLine($"[Response] Status Code: {context.Response.StatusCode}");
        }
    }
}
