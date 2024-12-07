using System.Threading.Tasks;
using TravelOrganization.Data.Models.Account;

namespace TravelOrganization.Data.Repositories.Account
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task<int> AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}
