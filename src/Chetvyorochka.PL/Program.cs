using Chetvyorochka.PL;
using Chetvyorochka.BL.Middlewares;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseMiddleware<ExceptionHandingMiddleware>();
app.UseMiddleware<ValidationMiddleware>();

app.UseStatusCodePagesWithReExecute("/Login/Index");

//app.UseSession();
app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["Token"];
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }

    await next();
});

//app.UseStatusCodePagesWithReExecute("/errorStatus/{0}");
app.Map("/errorStatus/{statusCode}", (int statusCode) =>
{
    if (statusCode == 401)
        app.Use(async (context, next) =>
        {
            context.Request.Path = "/Login/Index";
            await next();
        });
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

/*
app.Use(async (context, next) =>
{
    if (context.Response.StatusCode == 401)
        app.UseAuthentication();
    await next();
});
*/

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Seed(builder);

/*
app.Run(async context =>
{
    var userRequest = context.RequestServices.GetService<IUserRequest>();
    await userRequest.RegisterAsync(new RegisterDataModel
    {
        Login = "admin",
        FistName = "Админ",
        LastName = "Админов",
        Password = "1234"
    });
});
*/

app.Run();
