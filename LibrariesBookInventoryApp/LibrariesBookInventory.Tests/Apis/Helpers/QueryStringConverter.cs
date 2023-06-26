using System.Collections.Generic;
using System.Web;

namespace LibrariesBookInventory.Tests.Apis.Helpers
{
    public static class QueryStringConverter
    {
        public static string ToQueryString(object obj)
        {
            var parameters = new List<string>();

            foreach (var property in obj.GetType().GetProperties())
            {
                var value = property.GetValue(obj);
                if (value != null)
                {
                    var encodedValue = HttpUtility.UrlEncode(value.ToString());
                    var parameter = $"{property.Name}={encodedValue}";
                    parameters.Add(parameter);
                }
            }

            return string.Join("&", parameters);
        }
    }
}
