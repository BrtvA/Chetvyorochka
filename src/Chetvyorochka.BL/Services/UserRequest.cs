using Chetvyorochka.BL.CustomExceptions;
using Chetvyorochka.BL.Models;
using Chetvyorochka.DAL.Entities;
using Chetvyorochka.DAL.Models;
using Chetvyorochka.DAL.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Chetvyorochka.BL.Services
{
    public class UserRequest : IUserRequest
    {
        private readonly IUserDbRepository<User> _userDbRepository;
        private readonly ILogger<UserRequest> _logger;

        public UserRequest(IUserDbRepository<User> userDbRepository,
            ILogger<UserRequest> logger)
        {
            _userDbRepository = userDbRepository;
            _logger = logger;
        }

        public async Task AddMoneyAsync(string login, decimal moneyCount)
        {
            if (moneyCount <= 0)
            {
                throw new ArgumentOutOfRangeException("Сумма пополнения должна быть больше нуля");
            }

            User? user = await _userDbRepository.GetByNameAsync(login);
            if (user == null)
                throw new NotFoundException("Информация о пользователе не найдена");

            user.MoneyCount = user.MoneyCount + moneyCount;
            await _userDbRepository.SaveAsync();
        }

        public async Task<UserInfoModel?> GetUserInfoAsync(string login)
        {
            var userInfo = await _userDbRepository.GetUserInfoAsync(login);
            if (userInfo == null) 
                throw new NotFoundException("Информация о пользователе не найдена");

            return userInfo;
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }

        private string GenerateToken(string login, string userType)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, login), new Claim(ClaimTypes.Role, userType) };
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(1)), // время действия 1 минуты
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
            //return Tuple.Create(new JwtSecurityTokenHandler().WriteToken(jwt), new ClaimsIdentity(claims, "Cookies"));
        }

        public async Task<string> LoginAsync(LoginDataModel loginDataModel)
        {
            var authInfo = await _userDbRepository.GetAuthInfoAsync(loginDataModel.Login);
            if (authInfo == null) 
                throw new NotFoundException("Неверные аутентификационные данные");

            if (HashPassword(loginDataModel.Password) == authInfo.Password)
            {
                _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: Выполнен вход пользователя {loginDataModel.Login}");
                return GenerateToken(authInfo.Login, authInfo.UserType.ToString());
            }
            else
            {
                throw new BadRequestException("Неверный пароль");
            }
        }

        public async Task<string> RegisterAsync(RegisterDataModel registerDataModel)
        {
            await _userDbRepository.CreateAsync(new User
            {
                Login = registerDataModel.Login,
                Name = registerDataModel.FistName,
                LastName = registerDataModel.LastName,
                UserType = UserType.customer,
                MoneyCount = 0,
                Password = HashPassword(registerDataModel.Password)
            });
            await _userDbRepository.SaveAsync();

            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: Выполнена регистрация пользователя {registerDataModel.Login}");
            return GenerateToken(registerDataModel.Login, UserType.customer.ToString());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {

            if (disposed) return;
            if (disposing)
            {
                // Освобождаем управляемые ресурсы
                _userDbRepository.Dispose();
            }
            // освобождаем неуправляемые объекты
            disposed = true;
        }
        ~UserRequest()
        {
            Dispose(false);
        }
    }
}
