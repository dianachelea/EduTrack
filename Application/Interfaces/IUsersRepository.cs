using Domain;

namespace Application.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<UserCredentials>> GetUser(string email);
        Task<bool> RegisterUser(UserCredentials credentials);
        Task<bool> GiveUserAdminRights(string email);
        Task<bool> UpdatePassword(string email, string newPassword);
    }
}
