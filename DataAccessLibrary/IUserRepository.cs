using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IUserRepository
    {
        Task<List<UserModel>> GetUsers();
        Task InsertUser(UserModel user);
        Task<UserModel> GetUserById(string id);

    }
}