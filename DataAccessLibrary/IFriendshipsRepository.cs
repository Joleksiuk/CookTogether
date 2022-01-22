using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IFriendshipsRepository
    {
        Task<List<UserModel>> GetFriendListOfUser(string user);
        Task<List<UserModel>> GetNOTFriendedListOfUser(string user);
        Task<List<FriendshipModel>> GetFriendships();
        Task InsertFriendship(FriendshipModel friend);
        Task RemoveFriendship(UserModel friend1, UserModel friend2);
    }
}