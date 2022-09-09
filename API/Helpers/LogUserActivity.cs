using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using API.Extensions;
using API.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        //ActionExecutingContext - context of the action that is executing
        //ActionExecutionDelegate = Whats gonna happend next when the action is executed
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultConext = await next();
            if(!resultConext.HttpContext.User.Identity.IsAuthenticated) return;
            var userName = resultConext.HttpContext.User.GetUserName();
            var repo = context.HttpContext.RequestServices.GetService<IUserRepository>();
            var repoUser = await repo.GetUserAsync(userName);
            repoUser.LastActive = DateTime.Now;
            await repo.SaveAllAsync();
        }
    }
}