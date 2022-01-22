using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class MealRepository : IMealRepository
    {
        private readonly ISqlDataAccess _db;

        public MealRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task DeleteAllMeals()
        {
            string sql = @"delete from [dbo].[Meal]";
            return _db.SaveData(sql, new { });
        }

        public Task DeleteAllMealIngredients()
        {
            string sql = @"delete from [dbo].[RecipeIngredient]";
            return _db.SaveData(sql, new { });
        }

        public Task<List<MealModel>> GetMeals()
        {
            string sql = @"select * from [dbo].[Meal]";
            return _db.LoadData<MealModel, dynamic>(sql, new { });
        }

        public Task InsertMealIngredients(List<RecipeIngredientModel> mealIngredients)
        {
            string sql = @"insert into [dbo].[RecipeIngredient] (MealId, IngredientId, Measure)
                            values(@MealId, @IngredientId, @Measure)";
            return _db.SaveData(sql, mealIngredients);
        }

        public Task InsertMeals(List<MealModel> meals)
        {
            string sql = @"insert into [dbo].[Meal] (Id, Name, Recipe, ThumbnailUrl, CategoryId, AreaId)
                            values (@Id, @Name, @Recipe, @ThumbnailUrl, @CategoryId, @AreaId)";
            return _db.SaveData(sql, meals);
        }
    }
}
