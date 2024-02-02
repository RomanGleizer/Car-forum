using CarForum.Models;

namespace CarForum.DAO.AccountData;

public interface IAccountRepository
{
    Task<User> GetUserAsync(string userName);
    Task<User> GetUserByEmailAsync(string email);
    Task<bool> CreateUserAsync(User user, string password);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> SignInAsync(string userName, string password, bool rememberMe);
    Task SignOutAsync();
    Task<bool> ChangePasswordAsync(User user, string oldPassword, string newPassword);
}
