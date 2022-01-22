using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class UserRepository : IUserRepository
    {
        private readonly ISqlDataAccess _db;

        public UserRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<List<UserModel>> GetUsers()
        {
            string sql = @"select * from [dbo].[AspNetUsers]";
            return _db.LoadData<UserModel, dynamic>(sql, new { });
        }
        public Task<List<UserModel>> GetUsersExcludingUser(UserModel user)
        {
            string sql = @"select * from [dbo].[AspNetUsers] WHERE Id != '"+user.Id+"'";
            return _db.LoadData<UserModel, dynamic>(sql, new { });
        }

        public Task InsertUser(UserModel user)
        {
            string sql = @"insert into [dbo].[AspNetUsers] (Id, Username, PasswordHash)
                            VALUES (@Id, @UserName, @PasswordHash)";
            return _db.SaveData(sql, user);
        }

        public async Task<UserModel> GetUserById(string id)
        {
            List<UserModel> users = await GetUsers();
            UserModel foundUser = new UserModel();
            foreach(var user in users)
            {
                if(user.Id == id)
                {
                    foundUser = user;
                    break;
                }
            }
            return foundUser;
        }
    }
}
