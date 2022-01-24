using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.MealRepositories
{
    public interface ICategoryRepository
    {
        Task<List<CategoryModel>> GetCategories();
        Task InsertOrUpdateCategories(List<CategoryModel> categories);
        Task<CategoryModel> GetCategoryByName(string name);
        Task<CategoryModel> GetCategoryById(int CategoryId);
    }
}
