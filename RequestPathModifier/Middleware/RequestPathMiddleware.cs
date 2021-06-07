using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RequestPathModifier.Middleware
{
    public class RequestPathMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestPathMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string requestPath = context.Request.Path.HasValue ? context.Request.Path.Value : "/";

            if (TryMatchOnRegex("^/api/([A-Za-z]{1,50})/[a-zA-Z]+", requestPath, out Match match))
            {
                var matchStartIndex = match.Groups.Values.ElementAt(1).Index;
                var substringStartFrom = matchStartIndex + match.Groups.Values.ElementAt(1).Length;

                // transform /api/BLAH/values 
                // into      /api/values 
                var newPath =
                    requestPath.Substring(0, matchStartIndex-1) +
                    requestPath.Substring(substringStartFrom);

                context.Request.Path = new PathString(newPath);
                if (!context.Items.TryAdd("INTERNAL_CLIENT_ID", match.Groups[1].Value))
                {
                    // logging ...
                }
            }

            await _next.Invoke(context);

            // Clean up.

            // helper function
            static bool TryMatchOnRegex(string pattern, string requestPath, out Match match)
            {
                match = null;

                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                if (regex.IsMatch(requestPath))
                    match = regex.Match(requestPath);

                return match != null;
            }


        }
    }

    public static class RequestPathMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestPathMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestPathMiddleware>();
        }
    }

}
