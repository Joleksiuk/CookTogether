using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;

        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<List<UserModel>> GetUsers()
        {
            string sql = @"select * from [dbo].[AspNetUsers]";
            return _db.LoadData<UserModel, dynamic>(sql, new { });
        }

        public Task InsertUser(UserModel user)
        {
            string sql = @"insert into [dbo].[AspNetUsers] (Id, Username, PasswordHash)
                            VALUES (@Id, @UserName, @PasswordHash)";
            return _db.SaveData(sql, user);
        }
    }
}
