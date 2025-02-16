using HuntFlowWIUT.Web.Controllers;
using HuntFlowWIUT.Web.Extensions;
using HuntFlowWIUT.Web.Services;
using HuntFlowWIUT.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using static HuntFlowWIUT.Web.Services.Interfaces.IHuntFlowService;
using NLog.Web;


var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();



try
{
    logger.Debug("Init main");


    var builder = WebApplication.CreateBuilder(args);

    // Clear default logging providers and set minimum level
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

    // Use NLog for dependency injection
    builder.Host.UseNLog();

    var configuration = builder.Configuration;

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    builder.Services.AddSingleton<ICountryService, CountryService>();

    builder.Services.AddHttpClient<ITokenService, TokenService>(client =>
    {
        client.BaseAddress = new Uri(configuration["Huntflow:BaseUrl"]);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });

    builder.Services.AddHttpClient<IHuntflowService, HuntflowService>(client =>
    {
        client.BaseAddress = new Uri(configuration["Huntflow:BaseUrl"]);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();

}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}