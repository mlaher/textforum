using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using textforum.domain.interfaces;
using textforum.logic.helpers;
using textforum.logic.services;

namespace textforum.logic.filters
{
    public class UserAuthAttribute : AppAuthAttribute
    {
        private IUserAuthenticationService _userAuthenticationService = null;

        public UserAuthAttribute() : base()
        {
            
        }

        public UserAuthAttribute(IUserAuthenticationService userAuthenticationService, IAppAuthenticationService appAuthenticationService) : base(appAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
        }

        public override async void OnAuthorization(AuthorizationFilterContext context)
        {
            base.OnAuthorization(context);

            var correlationId = context.HttpContext.GetCorrelationId();

            var isValidToken = context.HttpContext.Request.Headers.TryGetValue("X-User-Token", out StringValues tokens);
            var token = tokens.FirstOrDefault();

            if (!isValidToken || string.IsNullOrWhiteSpace(token))
            {
                // user not authorized, redirect to login page
                context.Result = new JsonResult(new { message = "Unauthorized", correlationId }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            if (_userAuthenticationService == null)
            {
                _userAuthenticationService = context.HttpContext.RequestServices.GetService(typeof(IUserAuthenticationService)) as IUserAuthenticationService;
            }

            var result = await _userAuthenticationService.GetClaims(token, correlationId);

            if (!result.isValid)
            {
                context.Result = new JsonResult(new { message = "Unauthorized", correlationId = correlationId }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            foreach (var item in result.claims)
            {
                context.HttpContext.Items.Add(item.Key, item.Value);
            }            
        }
    }
}
