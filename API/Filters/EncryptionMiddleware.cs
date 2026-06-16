using API.Services;

namespace API.Filters;

public class EncryptionMiddleware
{
    private readonly RequestDelegate _next;

    public EncryptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IEncryptionService encryptionService)
    {
        var originalBodyStream = context.Response.Body;

        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        await _next(context);

        // Reset memory stream to read
        memoryStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

        bool isApiRoute = context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase);
        bool isGetRequest = context.Request.Method == HttpMethods.Get;

        if (isGetRequest && isApiRoute && context.Response.StatusCode == 200 && !string.IsNullOrEmpty(responseBody) && context.Response.ContentType?.Contains("application/json") == true)
        {
            var encryptedResponse = encryptionService.EncryptResponse(responseBody);
            
            // Clear the original response
            context.Response.Body = originalBodyStream;
            context.Response.ContentType = "text/plain"; // Change to plain text or application/json depending on client expectation. We'll use text/plain for the raw encrypted string.
            context.Response.ContentLength = encryptedResponse.Length;

            await context.Response.WriteAsync(encryptedResponse);
        }
        else
        {
            // If not 200 or not JSON, just copy it back as is
            memoryStream.Seek(0, SeekOrigin.Begin);
            context.Response.Body = originalBodyStream;
            await memoryStream.CopyToAsync(originalBodyStream);
        }
    }
}
