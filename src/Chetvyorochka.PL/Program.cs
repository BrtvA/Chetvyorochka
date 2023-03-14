using Chetvyorochka.PL;
using Chetvyorochka.BL.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Chetvyorochka.PL.Filters;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new MyAuthorizeAttribute());
});

builder.RegisterServices();

var app = builder.Build();

app.UseMiddleware<ExceptionHandingMiddleware>();
app.UseMiddleware<ValidationMiddleware>();

app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["Token"];
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }

    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Seed(builder);

app.Run();
