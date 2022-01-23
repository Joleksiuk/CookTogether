using Dapper;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class FriendshipsRepository : IFriendshipsRepository
    {
        private readonly ISqlDataAccess _db;

        public FriendshipsRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<List<FriendshipModel>> GetFriendships()
        {
            string sql = @"select * from [dbo].[AspFriendships]";
            return _db.LoadData<FriendshipModel, dynamic>(sql, new { });
        }

        public Task<List<UserModel>> GetFriendListOfUser(string Id)
        {
            string sql = @"SELECT users.Id, users.Username  FROM [dbo].[AspNetUsers] as users
                            WHERE users.Id IN (
                                SELECT [FirstUserID]
                                FROM [dbo].[AspFriendships]
                                WHERE [SecondUserID]=@UserId
                            UNION 
                                SELECT [SecondUserID]
                                FROM [dbo].[AspFriendships]
                                WHERE [FirstUserID]=@UserId)";
            return _db.LoadData<UserModel, dynamic>(sql, new { UserId = Id });
        }

        public Task<List<UserModel>> GetNotFriendedAndNotInvitedListOfUser(string Id, string UserName="")
        {
            string sql = @"SELECT users.Id, users.UserName  FROM [dbo].[AspNetUsers] as users
                            WHERE users.Id <> @UserId
                            AND users.Id NOT IN (
                                SELECT [FirstUserID]
                                FROM [dbo].[AspFriendships]
                                WHERE [SecondUserID]=@UserId
                                UNION 
                                SELECT [SecondUserID]
                                FROM [dbo].[AspFriendships]
                                WHERE [FirstUserID]=@UserId
                                UNION
                                SELECT [InvitedUserId]
                                FROM [FriendInvite]
                                WHERE [InvitingUserId] = @UserId)";
            if (UserName.Length > 0)
            {
                sql += "AND users.UserName LIKE @UserNameLike";
                 return _db.LoadData<UserModel, dynamic>(sql, new { UserId = Id, UserNameLike = "%" + UserName + "%" });
            }
            return _db.LoadData<UserModel, dynamic>(sql, new { UserId = Id });
        }

        public Task InsertFriendship(FriendshipModel friend)
        {
            string sql = @"insert into [dbo].[AspFriendships] (FirstUserId, SecondUserId)
                            VALUES (@FirstUserId, @SecondUserId)";
            return _db.SaveData(sql, friend);
        }

        public Task RemoveFriendship(UserModel friend1, UserModel friend2)
        {
            FriendshipModel friendshipModel = new FriendshipModel
            {
                FirstUserId = friend1.Id,
                SecondUserId = friend2.Id
            };
            string sql = @"Delete from AspFriendships   
                    WHERE  (FirstUserId = @FirstUserId AND SecondUserID = @SecondUserId) OR (FirstUserId = @SecondUserID AND SecondUserID = @FirstUserId)";

            return _db.SaveData(sql, friendshipModel);
        }

        public Task InsertFriendInvite(FriendInviteModel friendInvite)
        {
            string sql =
                @"INSERT INTO [dbo].[FriendInvite] (InvitingUserId, InvitedUserId)
                  VALUES (@InvitingUserId, @InvitedUserId)";
            return _db.SaveData(sql, friendInvite);
        }

        public Task<List<UserModel>> GetInvitingUsersByInvitedUserId(string userId)
        {
            string sql =
                @"SELECT [AspNetUsers].[Id], [AspNetUsers].[UserName] 
                  FROM [dbo].[AspNetUsers]
                  INNER JOIN [FriendInvite]
                  ON [FriendInvite].[InvitedUserId] = @InvitedUserId AND [FriendInvite].[InvitingUserId] = [AspNetUsers].[Id]";
            return _db.LoadData<UserModel, dynamic>(sql, new { InvitedUserId = userId });
        }

        public Task<List<UserModel>> GetInvitedUsersByInvitingUserId(string userId)
        {
            string sql =
                @"SELECT [AspNetUsers].[Id], [AspNetUsers].[UserName] 
                  FROM [dbo].[AspNetUsers]
                  INNER JOIN [FriendInvite]
                  ON [FriendInvite].[InvitingUserId] = @InvitingUserId AND [FriendInvite].[InvitedUserId] = [AspNetUsers].[Id]";
            return _db.LoadData<UserModel, dynamic>(sql, new { InvitingUserId = userId });
        }

        public Task RemoveFriendInvite(FriendInviteModel friendInvite)
        {
            string sql =
                @"DELETE FROM [dbo].[FriendInvite]
                  WHERE [FriendInvite].[InvitingUserId] = @InvitingUserId AND [FriendInvite].[InvitedUserId] = @InvitedUserId";
            return _db.SaveData(sql, friendInvite);
        }
    }
}

