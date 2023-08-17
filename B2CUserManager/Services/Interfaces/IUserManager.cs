
using B2CUserManager.Models;

namespace B2CUserManager.Services.Interfaces
{
    public interface IUserManager
    {
        Task<List<Profile>> GetUsers();

        Task<Profile> GetUserById(string id);

        Task<Profile> CreateUser(Profile user);

        Task<bool> DeleteUser(string id);

        Task<Profile> UpdateUser(Profile user);

    }
}
