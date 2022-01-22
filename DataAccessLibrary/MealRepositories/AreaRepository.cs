using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.MealRepositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly ISqlDataAccess _db;

        public AreaRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task DeleteAll()
        {
            string sql = @"delete from [dbo].[Area]";
            return _db.SaveData(sql, new { });
        }

        public Task<List<AreaModel>> GetAreaByName(string name)
        {
            string sql = @"select * from [dbo].[Area] where [dbo].[Area].[Name]=" + $"'{name}'";
            return _db.LoadData<AreaModel, dynamic>(sql, new { });
        }

        public Task<List<AreaModel>> GetAreas()
        {
            string sql = @"select * from [dbo].[Area]";
            return _db.LoadData<AreaModel, dynamic>(sql, new { });
        }

        public Task InsertAreas(List<AreaModel> areaModels)
        {
            string sql = @"insert into [dbo].[Area] (Id, Name) values (@Id, @Name)";
            return _db.SaveData(sql, areaModels);
        }
    }
}
