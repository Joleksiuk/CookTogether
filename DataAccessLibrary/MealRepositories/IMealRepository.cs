using DataAccessLibrary.Models;
using DataAccessLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.MealRepositories
{
    public interface IMealRepository
    {
        Task<List<MealModel>> GetMeals();
        Task<MealModel> GetMealById(int MealId);
        Task InsertOrUpdateMeals(List<MealModel> meals);
        Task InsertOrUpdateMealIngredients(List<RecipeIngredientModel> mealIngredients);
        Task<List<MealIngredientModel>> GetMealIngredientsById(int MealId);
        Task<List<MealModel>> GetMealsByCategoriesAndAreas(List<int> categoryIds, List<int> areaIds);
    }
}
