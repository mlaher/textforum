using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace textforum.logic.filters
{
    public class ModeratorAuthAttribute : UserAuthAttribute
    {
        public override async void OnAuthorization(AuthorizationFilterContext context)
        {
            base.OnAuthorization(context);

            var item = context.HttpContext.Items.FirstOrDefault(f => f.Key.ToString() == "ismoderator");
            var itemValue = item.Value as string;
            var isModerator = bool.TryParse(itemValue, out bool isMod) && isMod;

            if (!isModerator)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

        }
    }
}
