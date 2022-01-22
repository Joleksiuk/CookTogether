using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.MealRepositories
{
    public interface IIngredientRepository
    {
        Task<List<IngredientModel>> GetIngredients();
        Task<List<IngredientModel>> GetIngredientsByName(string name);
        Task InsertIngredients(List<IngredientModel> ingredients);
        Task DeleteAll();
    }
}
