using Chetvyorochka.BL.Models;
using Chetvyorochka.DAL.Models;
using System.Security.Claims;

namespace Chetvyorochka.BL.Services
{
    public interface IUserRequest:IDisposable
    {
        Task AddMoneyAsync(string login, decimal moneyCount);
        Task<string> LoginAsync(LoginDataModel loginDataModel);
        Task<string> RegisterAsync(RegisterDataModel registerDataModel);
        string HashPassword(string password);
        Task<UserInfoModel?> GetUserInfoAsync(string login);
    }
}