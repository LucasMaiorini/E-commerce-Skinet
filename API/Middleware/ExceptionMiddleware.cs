using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        /// <summary>
        /// This Middleware is defined to be set in Startup.
        /// It makes possible to call a custom error message when an exception occurs.
        /// </summary>
        /// <param name="next">Can process the Http Request</param>
        /// <param name="logger"></param>
        /// <param name="env"> Checks if is Development environment or not</param>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this._env = env;
            this._logger = logger;
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //if there is no exception the HttpRequest goes to next stage.
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                //All of our responses will be sent as json
                context.Response.ContentType = "application/json";
                //Makes the statusCode a 500 internal server error;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                //Is the environment is Development, the answer is more complete.
                var response = _env.IsDevelopment()
                ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                : new ApiException((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                //Converts the response in Json, cause we defined that our context Response must be json formatted.
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}