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

        public Task<AreaModel> GetAreaById(int AreaId)
        {
            string sql = @"SELECT * FROM [dbo].[Area] WHERE [Id] = @AreaId";
            return _db.LoadSingleResult<AreaModel, dynamic>(sql, new { AreaId = AreaId });
        }

        public Task<AreaModel> GetAreaByName(string name)
        {
            string sql = @"select * from [dbo].[Area] where [dbo].[Area].[Name]=@Name";
            return _db.LoadSingleResult<AreaModel, dynamic>(sql, new { Name = name });
        }

        public Task<List<AreaModel>> GetAreas()
        {
            string sql = @"select * from [dbo].[Area]";
            return _db.LoadData<AreaModel, dynamic>(sql, new { });
        }

        public Task InsertAreasIfNotExists(List<AreaModel> areaModels)
        {
            string sql =
                @"IF NOT EXISTS(SELECT * FROM [dbo].[Area] WHERE [dbo].[Area].[Name] = @Name)
                BEGIN
                    INSERT INTO[dbo].[Area] (Name)
                    VALUES(@Name)
                END";
            return _db.SaveData(sql, areaModels);
        }
    }
}
