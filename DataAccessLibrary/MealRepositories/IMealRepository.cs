using DataAccessLibrary.Models;
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
        Task InsertOrUpdateMeals(List<MealModel> meals);
        Task InsertOrUpdateMealIngredients(List<RecipeIngredientModel> mealIngredients);
    }
}
