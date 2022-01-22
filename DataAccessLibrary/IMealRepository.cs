using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IMealRepository
    {
        Task<List<MealModel>> GetMeals();
        Task InsertMeals(List<MealModel> meals);
        Task DeleteAllMeals();
        Task DeleteAllMealIngredients();

        Task InsertMealIngredients(List<RecipeIngredientModel> mealIngredients);
    }
}
