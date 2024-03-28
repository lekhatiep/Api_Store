using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn3API.Authorize
{
    public class CustomAuthorizationFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Specify the policy name
            var policyName = "CustomPolicy";
            var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();

            // Check if the user is authorized to the specified policy
            var authorizationResult = await authService.AuthorizeAsync(context.HttpContext.User, null, policyName);
            if (!authorizationResult.Succeeded)
            {
                // Handle unauthorized access
                context.Result = new ForbidResult();
            }
        }
    }
}
