using RAGChatBot.Domain.Entities;
using RAGChatBot.Infrastructure.DbContexts;
using RAGChatBot.Infrastructure.ResponseHelpers;
using System.Net;
using System.Text.Json;

namespace RAGChatBot.API.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            this.next = next;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                using var scope = serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();

                var exceptionLog = new ExceptionLog()
                {
                    Message = ex.Message,
                    Type = ex.GetType().Name,
                    StackTrace = ex.StackTrace ?? string.Empty,
                    URL = httpContext.Request?.Path,
                    CreatedDate = DateTime.UtcNow,
                };

                context.ExceptionLogs.Add(exceptionLog);
                await context.SaveChangesAsync();

                var response = new ErrorResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                };

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = response.StatusCode;

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
