using Villafane.WebSite.Middlewares;

namespace Villafane.WebSite.Extensions
{
    public static class ConfigurationCheckMiddlewareExtension
    {
        public static IApplicationBuilder UseConfigurationCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ConfigurationCheckMiddleware>();
        }
    }
}
