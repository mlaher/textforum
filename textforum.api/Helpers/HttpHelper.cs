namespace textforum.api.Helpers
{
    public class HttpHelper
    {
        public static long getUserId(HttpContext context, long userId) 
        {
            var item = context.Items.FirstOrDefault(f => f.Key.ToString() == "userid");
            var itemValue = item.Value as string;

            long.TryParse(itemValue, out userId);

            return userId;
        }
    }
}
