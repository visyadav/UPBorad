namespace API.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode statusCode;
        ApiResponse<object> response;

        switch (exception)
        {
            // FluentValidation pipeline throws this — surface the per-field errors
            case FluentValidation.ValidationException fluentEx:
                statusCode = HttpStatusCode.BadRequest;
                var fluentErrors = fluentEx.Errors.Select(e => e.ErrorMessage).ToList();
                response = ApiResponse<object>.FailureResponse(fluentErrors, "Validation failed.");
                break;

            // App-level ValidationException (list of strings)
            case ValidationException appEx:
                statusCode = HttpStatusCode.BadRequest;
                response = ApiResponse<object>.FailureResponse(appEx.Errors, "Validation failed.");
                break;

            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                response = ApiResponse<object>.FailureResponse([], exception.Message);
                break;

            case UnauthorizedException:
                statusCode = HttpStatusCode.Unauthorized;
                response = ApiResponse<object>.FailureResponse([], exception.Message);
                break;

            case ForbiddenException:
                statusCode = HttpStatusCode.Forbidden;
                response = ApiResponse<object>.FailureResponse([], exception.Message);
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;
                // Hide internal details from clients in production
                response = ApiResponse<object>.FailureResponse([], "An unexpected error occurred.");
                break;
        }

        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await context.Response.WriteAsync(json);
    }
}