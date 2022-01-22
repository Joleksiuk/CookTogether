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
        Task InsertCategories(List<CategoryModel> categories);
        Task<List<CategoryModel>> GetCategoryByName(string name);
        Task DeleteAll();
    }
}
