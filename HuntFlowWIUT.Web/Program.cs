using HuntFlowWIUT.Web.Controllers;
using HuntFlowWIUT.Web.Services;
using HuntFlowWIUT.Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<ITokenService, TokenService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["HuntFlow:BaseUrl"]);
});

builder.Services.AddHttpClient<IHuntFlowService, HuntFlowService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["HuntFlow:BaseUrl"]);
    client.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
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
