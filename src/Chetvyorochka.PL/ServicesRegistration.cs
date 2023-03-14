using Chetvyorochka.DAL.Entities;
using Chetvyorochka.DAL.Repositories;
using Chetvyorochka.DAL;
using Microsoft.EntityFrameworkCore;
using Chetvyorochka.BL.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Chetvyorochka.PL.Filters;

namespace Chetvyorochka.PL
{
    public static class ServicesRegistration
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("ProductDB")));
            builder.Services.AddScoped<IUserDbRepository<User>, UserDbRepository>();
            builder.Services.AddScoped<IProductDbRepository<Product>, ProductDbRepository>();
            builder.Services.AddScoped<IBasketDbRepository<Basket>, BasketDbRepository>();
            builder.Services.AddScoped<IProductTypeDbRepository<ProductType>, ProductTypeDbRepository>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            
            builder.Services.AddScoped<IBasketRequest, BasketRequest>();
            builder.Services.AddScoped<IProductRequest, ProductRequest>();
            builder.Services.AddScoped<IUserRequest, UserRequest>();
            builder.Services.AddScoped<IProductTypeRequest, ProductTypeRequest>();
        }
    }
}
