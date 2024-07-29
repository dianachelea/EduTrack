using Domain;

namespace Application.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<UserInfo>> GetUserInfo(string email);
        Task<bool> RegisterUser(UserCredentials credentials);
        Task<bool> GiveUserAdminRights(string email);
        Task<bool> UpdatePassword(string email, string newPassword);
        Task<IEnumerable<UserCredentials>> GetAllStudents();
    }
}
