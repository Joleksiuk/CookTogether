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

        public Task<List<IngredientModel>> GetIngredientsByNames(List<string> names)
        {
            string sql = @"select * from [dbo].[Ingredient]
                            where [dbo].[Ingredient].[Name] IN @Names";
            return _db.LoadData<IngredientModel, dynamic>(sql, new { Names = names });
        }

        public Task<List<IngredientModel>> GetIngredients()
        {
            string sql = @"select * from [dbo].[Ingredient]";
            return _db.LoadData<IngredientModel, dynamic>(sql, new { });
        }

        public Task InsertOrUpdateIngredients(List<IngredientModel> categories)
        {
            string sql =
            @"IF EXISTS(SELECT * FROM [dbo].[Ingredient] WHERE [dbo].[Ingredient].[Id] = @Id)
                BEGIN
                    UPDATE[dbo].[Ingredient] SET [Name] = @Name, [ThumbnailSmallUrl]= @ThumbnailSmallUrl
                    WHERE[Id] = @Id
                END
            ELSE
                BEGIN
                    INSERT INTO[dbo].[Ingredient] (Id, ThumbnailSmallUrl, Name)
                    VALUES(@Id, @ThumbnailSmallUrl, @Name)
                END";
            return _db.SaveData(sql, categories);
        }
    }
}
