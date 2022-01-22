using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.MealRepositories
{
    public class IngredientRepository : IIngredientRepository
    {
        ISqlDataAccess _db;

        public IngredientRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task DeleteAll()
        {
            string sql = @"delete from [dbo].[Ingredient]";
            return _db.SaveData(sql, new { });
        }

        public Task<List<IngredientModel>> GetIngredientsByName(string name)
        {
            string sql = @"select * from [dbo].[Ingredient]
                            where [dbo].[Ingredient].[Name]=" + $"'{name}'";
            return _db.LoadData<IngredientModel, dynamic>(sql, new { });
        }

        public Task<List<IngredientModel>> GetIngredients()
        {
            string sql = @"select * from [dbo].[Ingredient]";
            return _db.LoadData<IngredientModel, dynamic>(sql, new { });
        }

        public Task InsertIngredients(List<IngredientModel> categories)
        {
            string sql = @"insert into [dbo].[Ingredient] (Id, Name, ThumbnailUrl) values (@Id, @Name, @ThumbnailUrl)";
            return _db.SaveData(sql, categories);
        }
    }
}
