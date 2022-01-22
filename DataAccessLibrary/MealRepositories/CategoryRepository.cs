using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.MealRepositories
{
    public class CategoryRepository : ICategoryRepository
    {
        ISqlDataAccess _db;

        public CategoryRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task DeleteAll()
        {
            string sql = @"delete from [dbo].[Category]";
            return _db.SaveData(sql, new { });
        }

        public Task<List<CategoryModel>> GetCategories()
        {
            string sql = @"select * from [dbo].[Category]";
            return _db.LoadData<CategoryModel, dynamic>(sql, new { });
        }

        public Task<List<CategoryModel>> GetCategoryByName(string name)
        {
            string sql = @"select * from [dbo].[Category] where [dbo].[Category].[Name]=" + $"'{name}'";
            return _db.LoadData<CategoryModel, dynamic>(sql, new { });
        }

        public Task InsertCategories(List<CategoryModel> categories)
        {
            string sql = @"insert into [dbo].[Category] (Id, Name, ThumbnailUrl) values (@Id, @Name, @ThumbnailUrl)";
            return _db.SaveData(sql, categories);
        }
    }
}
