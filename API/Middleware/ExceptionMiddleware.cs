using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        // Middleware uses concept of a pipeline, in particular, http request pipeline
        // It does something it needs to and then calls the next action in the chain
        // So we need a "pointer" to the next step
        private readonly RequestDelegate _nextRequestStep;

        private readonly ILogger<ExceptionMiddleware> _logger;

        private readonly IHostEnvironment _environment;

        public ExceptionMiddleware(RequestDelegate nextRequestStep, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            _nextRequestStep = nextRequestStep;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // If sh** goes south somewhere down the call stack,
                // the exception will bubble up to this level...
                await _nextRequestStep.Invoke(httpContext);
            }
            catch (Exception e)
            {
                // ...and we handle (log) the exception here
                _logger.LogError(e, e.Message);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var response = _environment.IsDevelopment()
                    ? new ApiException(httpContext.Response.StatusCode, e.Message, e.StackTrace)
                    : new ApiException(httpContext.Response.StatusCode, "Internal server error.");

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                //var jsonResponse = JsonSerializer.Serialize(response, jsonOptions);
                await httpContext.Response.WriteAsJsonAsync(response, jsonOptions);
            }
        }
    }
}
