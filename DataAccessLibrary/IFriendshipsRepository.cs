using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IFriendshipsRepository
    {
        Task<List<UserModel>> GetFriendListOfUser(string user);
        Task<List<UserModel>> GetNotFriendedAndNotInvitedListOfUser(string user, string UserName = "");
        Task<List<FriendshipModel>> GetFriendships();
        Task InsertFriendship(FriendshipModel friend);
        Task RemoveFriendship(UserModel friend1, UserModel friend2);

        Task<List<UserModel>> GetInvitingUsersByInvitedUserId(string userId);
        Task<List<UserModel>> GetInvitedUsersByInvitingUserId(string userId);
        Task InsertFriendInvite(FriendInviteModel friendInvite);
        Task RemoveFriendInvite(FriendInviteModel friendInvite);
    }
}