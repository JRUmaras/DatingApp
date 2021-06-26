using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace API.Helpers
{
    public class UserActivityLogger : IAsyncActionFilter
    {
        private readonly ILogger<UserActivityLogger> _logger;

        public UserActivityLogger(ILogger<UserActivityLogger> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            var identity = resultContext.HttpContext.User.Identity;
            if (identity is null || !identity.IsAuthenticated) return;

            var username = resultContext.HttpContext.User.GetUsername();
            var userRepo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            if (userRepo is null)
            {
                _logger.Log(LogLevel.Warning, "Failed to inject user repository.");
                return;
            }

            var user = await userRepo.GetUserByUsernameAsync(username);
            user.LastActive = DateTime.UtcNow;
            await userRepo.SaveAllAsync();
        }
    }
}
