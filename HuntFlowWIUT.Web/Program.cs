using HuntFlowWIUT.Web.Controllers;
using HuntFlowWIUT.Web.Services;
using HuntFlowWIUT.Web.Services.Interfaces;
using static HuntFlowWIUT.Web.Services.Interfaces.IHuntFlowService;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

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
