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

        public Task<List<CategoryModel>> GetCategories()
        {
            string sql = @"select * from [dbo].[Category]";
            return _db.LoadData<CategoryModel, dynamic>(sql, new { });
        }

        public Task<CategoryModel> GetCategoryByName(string name)
        {
            string sql = @"select * from [dbo].[Category] where [dbo].[Category].[Name]=@Name";
            return _db.LoadSingleResult<CategoryModel, dynamic>(sql, new { Name = name });
        }

        public Task InsertOrUpdateCategories(List<CategoryModel> categories)
        {
            string sql =
                @"IF EXISTS(SELECT * FROM [dbo].[Category] WHERE [dbo].[Category].[Id] = @Id)
                    BEGIN
                        UPDATE[dbo].[Category] SET [Name] = @Name, [ThumbnailUrl]= @ThumbnailUrl
                        WHERE[Id] = @Id
                    END
                ELSE
                    BEGIN
                        INSERT INTO[dbo].[Category] (Id, ThumbnailUrl, Name)
                        VALUES(@Id, @ThumbnailUrl, @Name)
                    END";
            return _db.SaveData(sql, categories);
        }
    }
}
