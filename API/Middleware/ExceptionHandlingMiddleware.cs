namespace API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode statusCode;

        object response;

        switch (exception)
        {
            case ValidationException validationException:

                statusCode = HttpStatusCode.BadRequest;

                response = ApiResponse<object>
                    .FailureResponse(
                        [],
                        validationException.Message);

                break;

            case NotFoundException:

                statusCode = HttpStatusCode.NotFound;

                response = ApiResponse<object>
                    .FailureResponse(
                        [],
                        exception.Message);

                break;

            case UnauthorizedException:

                statusCode = HttpStatusCode.Unauthorized;

                response = ApiResponse<object>
                    .FailureResponse(
                        [],
                        exception.Message);

                break;

            default:

                statusCode = HttpStatusCode.InternalServerError;

                response = ApiResponse<object>
                    .FailureResponse(
                        [],
                        "Internal Server Error");

                break;
        }

        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(result);
    }
}