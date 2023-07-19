using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Villafane.Domain.Models.DB;

namespace Villafane.WebSite.Middlewares
{
    internal class ConfigurationCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public ConfigurationCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, VillafaneContext dbContext)
        {
            string currentRoute = context.Request.Path;
            if (await dbContext.Configuracions.CountAsync() == 0 && !currentRoute.Contains("Configuration")
                && !IsStaticFileRequest(context.Request))
            {
                context.Response.Redirect("/Configuration/Index");
                return;
            }

            await _next(context);
        }

        private bool IsStaticFileRequest(HttpRequest request)
        {
            return request.Path.Value.Contains(".");
        }
    }
}

