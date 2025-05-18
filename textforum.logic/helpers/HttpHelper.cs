using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace textforum.logic.helpers
{
    public static class HttpHelper
    {
        public static long GetUserId(this HttpContext context, long userId)
        {
            var item = context.Items.FirstOrDefault(f => f.Key.ToString() == "userid");
            var itemValue = item.Value as string;

            long.TryParse(itemValue, out userId);

            return userId;
        }

        public static string GetCorrelationId(this HttpContext context)
        {
            context.Items ??= new Dictionary<object, object?>();

            if(!context.Items.ContainsKey("CorrelationId"))
                context.Items.Add("CorrelationId", Guid.NewGuid().ToString());

            context.Items.TryGetValue("CorrelationId", out object? result);

            var correlationId = result?.ToString();

            if (!context.Response.Headers.ContainsKey("CorrelationId"))
                context.Response.Headers.Add("CorrelationId", correlationId);

            return correlationId ?? "";
        }
    }
}
