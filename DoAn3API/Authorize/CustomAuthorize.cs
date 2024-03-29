using DoAn3API.Services.Users;
using Infastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DoAn3API.Authorize.CustomAuthorize
{
    public class CustomAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {

        private readonly IUserService userService;
        private string policyName = "";

        public CustomAuthorize(
            IUserService userService
            )
        {
            this.userService = userService;
        }

        public CustomAuthorize(
          string permissionName
           )
        {
            policyName = permissionName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
            }

            var dbContext = context.HttpContext.RequestServices.GetService(typeof(AppDbContext)) as AppDbContext;
            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

            var identity = context.HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userID = int.Parse(identity.FindFirst("id").Value);

                Task.Run(async () => {

                    var permission = await userService.GetAllPermissionByUserId(userID);

                    if (!permission.Any(x => x == policyName))
                    {
                        context.Result = new JsonResult(new
                        {
                            Message = "Request Access Denied"
                        })
                        {
                            StatusCode = StatusCodes.Status401Unauthorized
                        };
                    }

                }).GetAwaiter().GetResult();
            }
                
        }

    }
}
