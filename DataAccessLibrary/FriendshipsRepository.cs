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
                                WHERE [SecondUserID] = " + $"'{Id}'" +
                            "UNION " +
                            "SELECT [SecondUserID]" +
                            "FROM [dbo].[AspFriendships]" +
                            "WHERE [FirstUserID] = " + $"'{Id}')";
            return _db.LoadData<UserModel, dynamic>(sql, new { });
        }

        public Task<List<UserModel>> GetNOTFriendedListOfUser(string Id)
        {
            string sql = @"SELECT users.Id, users.Username  FROM [dbo].[AspNetUsers] as users
                            WHERE  users.Id <> " + $"'{Id}' " +
                            @"AND users.Id NOT IN (
                                SELECT [FirstUserID]
                                FROM [dbo].[AspFriendships]
                                WHERE [SecondUserID] = " + $"'{Id}'" +
                                @"UNION 
                                SELECT [SecondUserID]
                                FROM [dbo].[AspFriendships]
                                WHERE [FirstUserID] = " + $"'{Id}')"
                              ;
            return _db.LoadData<UserModel, dynamic>(sql, new { });
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
    }
}

