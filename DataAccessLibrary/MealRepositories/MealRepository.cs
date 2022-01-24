using DataAccessLibrary.Models;
using DataAccessLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.MealRepositories
{
    public class MealRepository : IMealRepository
    {
        private readonly ISqlDataAccess _db;

        public MealRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<List<MealModel>> GetMeals()
        {
            string sql = @"select * from [dbo].[Meal]";
            return _db.LoadData<MealModel, dynamic>(sql, new { });
        }

        public Task<MealModel> GetMealById(int MealId)
        {
            string sql = @"SELECT * FROM [dbo].[Meal]
                           WHERE [Id] = @MealId";
            return _db.LoadSingleResult<MealModel, dynamic>(sql, new { MealId = MealId });
        }

        public Task<List<MealIngredientModel>> GetMealIngredientsById(int MealId)
        {
            string sql = @"SELECT [Ingredient].[Name], [Ingredient].[ThumbnailUrl], [RecipeIngredient].[Measure]
                           FROM [dbo].[Ingredient]
                           INNER JOIN [dbo].[RecipeIngredient]
                           ON [RecipeIngredient].[MealId] = @MealId
                           AND [RecipeIngredient].[IngredientId] = [Ingredient].[Id]";
            return _db.LoadData<MealIngredientModel, dynamic>(sql, new { MealId = MealId });
        }

        public Task InsertOrUpdateMealIngredients(List<RecipeIngredientModel> mealIngredients)
        {
            string sql =
            @"IF EXISTS(SELECT * FROM [dbo].[RecipeIngredient] WHERE [MealId] = @MealId AND [IngredientId] = @IngredientId)
                BEGIN
                    UPDATE [dbo].[RecipeIngredient] 
                    SET [Measure] = @Measure
                    WHERE [MealId] = @MealId AND [IngredientId] = @IngredientId
                END
            ELSE
                BEGIN
                    INSERT INTO [dbo].[RecipeIngredient] (MealId, IngredientId, Measure)
                            values(@MealId, @IngredientId, @Measure)
                END";

            return _db.SaveData(sql, mealIngredients);
        }

        public Task InsertOrUpdateMeals(List<MealModel> meals)
        {
            string sql =
                @"IF EXISTS(SELECT * FROM [dbo].[Meal] WHERE [dbo].[Meal].[Id] = @Id)
                    BEGIN
                        UPDATE [dbo].[Meal] 
                        SET [Name] = @Name,
                            [Recipe] = @Recipe,
                            [ThumbnailUrl] = @ThumbnailUrl,
                            [CategoryId] = @CategoryId,
                            [AreaId] = @AreaId
                        WHERE [Id] = @Id
                    END
                ELSE
                    BEGIN
                        INSERT INTO [dbo].[Meal] (Id, Name, Recipe, ThumbnailUrl, CategoryId, AreaId)
                            VALUES (@Id, @Name, @Recipe, @ThumbnailUrl, @CategoryId, @AreaId)
                    END";
            return _db.SaveData(sql, meals);
        }
    }
}
