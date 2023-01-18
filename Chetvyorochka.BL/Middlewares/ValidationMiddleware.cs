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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chetvyorochka.BL.Middlewares
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            bool following = true;

            string regLatinNumber = @"^[A-Za-z0-9]\S+$";
            string regCyrillic = @"^[А-Яа-яЁё]+$";
            string regCyrillicSpace = @"^[А-Яа-яЁё ]+$";
            string regCyrillicLatinSpace = "^[A-Za-zА-Яа-яЁё\" ]+$";
            string regCyrillicNumberSpace = @"^[А-Яа-яЁё0-9 ]+$";

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
                        following = CheckString(5, 20, loginData.Login.Trim(), regLatinNumber) & CheckString(4, 20, loginData.Password.Trim(), regLatinNumber);
                        break;
                    case "/Register":
                        RegisterDataModel? registerData = await request.ReadFromJsonAsync<RegisterDataModel>();
                        following = CheckString(5, 20, registerData.Login.Trim(), regLatinNumber)
                                 & CheckString(1, 20, registerData.FistName.Trim(), regCyrillic)
                                 & (CheckString(1, 20, registerData.LastName.Trim(), regCyrillic) | CheckLength(0, 0, registerData.LastName.Trim().Length))
                                 & CheckString(4, 20, registerData.Password.Trim(), regLatinNumber);
                        break;
                    case "/User/AddMoney":
                        decimal money = await request.ReadFromJsonAsync<decimal>();
                        following = CheckLength(1, 900000, money);
                        break;
                    case "/ProductType/Add":
                    case "/ProductType/Edit":
                        ProductType? productType = await request.ReadFromJsonAsync<ProductType>();
                        following = CheckString(3, 20, productType.Name.Trim(), regCyrillicSpace);
                        break;
                    case "/Product/Add":
                    case "/Product/Edit":
                        Product? product = await request.ReadFromJsonAsync<Product>();
                        following = CheckString(3, 50, product.Name.Trim(), regCyrillicLatinSpace)
                                 & CheckString(3, 50, product.Description.Trim(), regCyrillicNumberSpace)
                                 & CheckLength(1, 900000, product.Price)
                                 & CheckLength(1, 1000000, (decimal)product.Count);
                        break;
                    default:
                        break;
                }
                request.Body.Position = 0;
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
    }
}