using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using Villafane.Domain.Models.DB;
using Villafane.Services.DB;
using Villafane.WebSite.Binder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Villafane.WebSite.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new CustomBinderProvider());
}).AddRazorRuntimeCompilation(); ;

if (builder.Environment.IsDevelopment())
{
    var connectionString = builder.Configuration.GetConnectionString("VillafaneDBDev");
    builder.Services.AddDbContext<VillafaneContext>(x => x.UseSqlServer(connectionString));
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("VillafaneDBProd");
    builder.Services.AddDbContext<VillafaneContext>(x => x.UseSqlServer(connectionString));
}
builder.Services.AddScoped<GrupoDeInteresBusinessManager>();
builder.Services.AddScoped<ValoresBusinessManager>();
builder.Services.AddScoped<VariablesBusinessManager>();
builder.Services.AddScoped<IndicadoresBusinessManager>();
builder.Services.AddScoped<ConfigurationBusinessManager>();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
            
            new CultureInfo("es")
        };
    options.DefaultRequestCulture = new RequestCulture(culture: "es-ES", uiCulture: "es-ES");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueCountLimit = int.MaxValue;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    var currentThreadCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
    currentThreadCulture.NumberFormat = NumberFormatInfo.InvariantInfo;

    Thread.CurrentThread.CurrentCulture = currentThreadCulture;
    Thread.CurrentThread.CurrentUICulture = currentThreadCulture;

    await next();
});

app.UseConfigurationCheck();
app.UseRequestLocalization();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
