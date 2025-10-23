// NOTE. https://learn.microsoft.com/ko-kr/aspnet/core/fundamentals/middleware/write?view=aspnetcore-8.0
using ShipParticularsApi.Exceptions;

namespace ShipParticularsApi.Middlewares
{
    public class GlobalExceptionMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            string title;
            string message = exception.Message;

            switch (exception)
            {
                case BadRequestException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    title = "Bad Request";
                    break;
                case ResourceNotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    title = "Resource Not Found";
                    break;
                case DatabaseConstraintException:
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    title = "Database Conflict";
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    title = "Internal Server Error";
                    message = "서버 처리 중 예기치 않은 오류가 발생했습니다. 관리자에게 문의하세요.";
                    break;
            }

            var response = new
            {
                Title = title,
                Message = message
            };

            var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
