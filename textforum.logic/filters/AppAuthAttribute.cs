using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using textforum.domain.interfaces;

namespace textforum.logic.filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AppAuthAttribute : Attribute, IAuthorizationFilter
    {
        private IAppAuthenticationService _appAuthenticationService = null;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_appAuthenticationService == null)
            {
                _appAuthenticationService = context.HttpContext.RequestServices.GetService(typeof(IAppAuthenticationService)) as IAppAuthenticationService;
            }

            var isValidToken = context.HttpContext.Request.Headers.TryGetValue("X-Token", out StringValues tokens);
            var token = tokens.FirstOrDefault();

            if (!isValidToken || string.IsNullOrWhiteSpace(token))
            {
                // user not authorized, redirect to login page
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };

                return;
            }

            string ip = "";
            if (context.HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var ipHeaderValues))
            {
                var ipList = ipHeaderValues.FirstOrDefault();
                ip = ipList != null ? ipList.Split(',').FirstOrDefault() : string.Empty;
            }

            string machineName = "";
            if (context.HttpContext.Request.Headers.TryGetValue("X-Machine-Name", out var machineNameValues))
            {
                var machineList = machineNameValues.FirstOrDefault();
                machineName = machineList != null ? machineList.Split(',').FirstOrDefault() : string.Empty;
            }

            if (!_appAuthenticationService.AuthenticateApp(token, ip, machineName))
            {
                // user not authorized, redirect to login page
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };

                return;
            }
        }
    }
}
