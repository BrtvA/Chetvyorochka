using Chetvyorochka.BL.CustomExceptions;
using Chetvyorochka.BL.Models;
using Chetvyorochka.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chetvyorochka.BL.Middlewares
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        private const string REG_LATIN_NUMBER = @"^[A-Za-z0-9]\S+$";
        private const string REG_CYRILLIC = @"^[А-Яа-яЁё]+$";
        private const string REG_CYRILLIC_SPACE = @"^[А-Яа-яЁё ]+$";
        private const string REG_CYRILLIC_LATIN_SPACE = "^[A-Za-zА-Яа-яЁё\" ]+$";
        private const string REG_CYRILLIC_NUMBER_POINT_SPACE = @"^[А-Яа-яЁё0-9. ]+$";

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            bool following = true;

            var request = httpContext.Request;
            var reqMethod = request.Method;
            var uri = request.Path;

            if (reqMethod == "POST")
            {
                request.EnableBuffering();
                switch (uri)
                {
                    case "/Login":
                        LoginDataModel? loginData = await request.ReadFromJsonAsync<LoginDataModel>();
                        loginData.Login = loginData.Login.Trim();
                        loginData.Password= loginData.Password.Trim();

                        following = CheckString(5, 20, loginData.Login, REG_LATIN_NUMBER) & CheckString(4, 20, loginData.Password, REG_LATIN_NUMBER);

                        AddToRequest(request, loginData);
                        break;
                    case "/Register":
                        RegisterDataModel? registerData = await request.ReadFromJsonAsync<RegisterDataModel>();
                        registerData.Login = registerData.Login.Trim();
                        registerData.FistName = registerData.FistName.Trim();
                        registerData.LastName = registerData.LastName.Trim();
                        registerData.Password = registerData.Password.Trim();

                        following = CheckString(5, 20, registerData.Login, REG_LATIN_NUMBER)
                                 & CheckString(1, 20, registerData.FistName, REG_CYRILLIC)
                                 & (CheckString(1, 20, registerData.LastName, REG_CYRILLIC) | CheckLength(0, 0, registerData.LastName.Length))
                                 & CheckString(4, 20, registerData.Password, REG_LATIN_NUMBER);

                        AddToRequest(request, registerData);
                        break;
                    case "/User/AddMoney":
                        decimal money = await request.ReadFromJsonAsync<decimal>();
                        following = CheckLength(1, 900000, money);

                        request.Body.Position = 0;
                        break;
                    case "/ProductType/Add":
                    case "/ProductType/Edit":
                        ProductType? productType = await request.ReadFromJsonAsync<ProductType>();
                        productType.Name = productType.Name.Trim();

                        following = CheckString(3, 20, productType.Name, REG_CYRILLIC_SPACE);

                        AddToRequest(request, productType);
                        break;
                    case "/Product/Add":
                    case "/Product/Edit":
                        Product? product = await request.ReadFromJsonAsync<Product>();
                        product.Name = product.Name.Trim();
                        product.Description = product.Description.Trim();

                        following = CheckString(3, 50, product.Name.Trim(), REG_CYRILLIC_LATIN_SPACE)
                                 & CheckString(3, 50, product.Description.Trim(), REG_CYRILLIC_NUMBER_POINT_SPACE)
                                 & CheckLength(1, 900000, product.Price)
                                 & CheckLength(1, 1000000, (decimal)product.Count);

                        AddToRequest(request, product);
                        break;
                    default:
                        break;
                }
            }

            if (following) 
                await _next(httpContext);
            else
                throw new BadRequestException("Валидация данных не пройдена");
        }

        private bool CheckString(int minLength, int maxLength, string value, string regex)
        {
            return CheckLength(minLength, maxLength, (decimal)value.Length) & Validate(value, regex);
        }

        private bool CheckLength(int minLength, int maxLength, decimal valueLength)
        {
            return valueLength >= minLength & valueLength <= maxLength;
        }

        private bool Validate(string value, string regex)
        {
            return Regex.IsMatch(value, regex);
        }

        private void AddToRequest(HttpRequest request, object dataSource)
        {
            string json = JsonSerializer.Serialize(dataSource);
            byte[] requestData = Encoding.UTF8.GetBytes(json);
            request.Body = new MemoryStream(requestData);
        }
    }
}