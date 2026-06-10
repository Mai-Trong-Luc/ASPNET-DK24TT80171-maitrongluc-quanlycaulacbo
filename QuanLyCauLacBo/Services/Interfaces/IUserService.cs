using QuanLyCauLacBo.Models.Domain;

namespace QuanLyCauLacBo.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> AuthenticateAsync(string username, string password);
        Task<User> RegisterAsync(User user, string plainPassword);
        Task<User> UpdateProfileAsync(User user);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> SetUserActiveAsync(int userId, bool isActive);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
    }
}
