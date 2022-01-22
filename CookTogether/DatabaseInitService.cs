using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookTogether.Models.Api;
using DataAccessLibrary;
using DataAccessLibrary.MealRepositories;
using DataAccessLibrary.Models;

namespace CookTogether
{
    public class DatabaseInitService
    {
        private readonly MealRepositories repositories;
        private readonly MealApiService mealApiService;

        public DatabaseInitService(MealApiService mealApiService, MealRepositories repositories)
        {
            this.repositories = repositories;
            this.mealApiService = mealApiService;
        }

        public async Task RunInit()
        {
            await repositories.MealRepository.DeleteAllMealIngredients();
            await repositories.MealRepository.DeleteAllMeals();

            await Task.WhenAll(
                InitAreas(),
                InitCategories(),
                InitIngredients());
            await InitMeals();
        }

        private async Task InitAreas()
        {
            Area[] areas = await mealApiService.GetAreas();
            
            List<AreaModel> areaModels = new();
            int Id = 0;
            foreach(Area area in areas)
            {
                areaModels.Add(new AreaModel()
                {
                    Id = Id++,
                    Name = area.Name
                });
            }
            await repositories.AreaRepository.DeleteAll();
            await repositories.AreaRepository.InsertAreas(areaModels);
        }

        private async Task InitCategories()
        {
            Category[] categories = await mealApiService.GetCategories();
            List<CategoryModel> categoryModels = new();
            foreach(Category category in categories)
            {
                categoryModels.Add(new CategoryModel()
                {
                    Id = category.Id,
                    Name = category.Name,
                    ThumbnailUrl = category.ThumbnailUrl.ToString()
                });
            }
            await repositories.CategoryRepository.DeleteAll();
            await repositories.CategoryRepository.InsertCategories(categoryModels);
        }

        private async Task InitIngredients()
        {
            Ingredient[] ingredients = await mealApiService.GetIngredients();
            List<IngredientModel> ingredientModels = new();
            foreach(Ingredient ingredient in ingredients)
            {
                ingredientModels.Add(new IngredientModel()
                {
                    Id = int.Parse(ingredient.Id),
                    Name = ingredient.Name,
                    ThumbnailUrl = ingredient.ThumbnailUrl.ToString()
                });
            }
            await repositories.IngredientRepository.DeleteAll();
            await repositories.IngredientRepository.InsertIngredients(ingredientModels);
        }

        private async Task InitMeals()
        {
            List<Meal> allMeals = new();
            List<MealModel> mealModels = new();
            List<RecipeIngredientModel> recipeIngredientModels = new();

            for (char starting = 'a'; starting <= 'z'; starting++)
            {
                Meal[] mealsStartingWith = await mealApiService.GetMealsStartingWith(starting.ToString());
                allMeals.AddRange(mealsStartingWith);
            }

            foreach(Meal meal in allMeals)
            {
                List<CategoryModel> categoryList = await repositories.CategoryRepository.GetCategoryByName(meal.CategoryName);
                List<AreaModel> areaList = await repositories.AreaRepository.GetAreaByName(meal.AreaName);

                CategoryModel category = categoryList.FirstOrDefault();
                AreaModel area = areaList.FirstOrDefault();
                List<RecipeIngredientModel> recipeIngredients = await CreateIngredientModelsForMeal(meal);

                mealModels.Add(new MealModel()
                {
                    Id = int.Parse(meal.Id),
                    Name = meal.Name,
                    Recipe = meal.Recipe,
                    ThumbnailUrl = meal.ThumbnailUrl.ToString(),
                    CategoryId = category.Id,
                    AreaId = area.Id
                });

                recipeIngredientModels.AddRange(recipeIngredients);
            }

            await repositories.MealRepository.InsertMeals(mealModels);
            await repositories.MealRepository.InsertMealIngredients(recipeIngredientModels);      
        }

        private async Task<List<RecipeIngredientModel>> CreateIngredientModelsForMeal(Meal meal)
        {
            List<string> ingredientNames = meal.GetIngredientList();
            List<string> ingredientMeasures = meal.GetMeasureList();
            List<RecipeIngredientModel> recipeIngredientModels = new();


            ingredientNames.RemoveAll(name => name == String.Empty);
            ingredientMeasures.RemoveAll(measure => measure == String.Empty);

            List<IngredientModel> ingredientModels = new();
            foreach(string ingredientName in ingredientNames)
            {
                List<IngredientModel> ingredients = await repositories.IngredientRepository.GetIngredientsByName(ingredientName);
                if (ingredients.FirstOrDefault() != default)
                {
                    ingredientModels.Add(ingredients.First());
                }
            }

            foreach(var (model, measure) in ingredientModels.Zip(ingredientMeasures))
            {
                recipeIngredientModels.Add(new RecipeIngredientModel()
                {
                    IngredientId = model.Id,
                    MealId = int.Parse(meal.Id),
                    Measure = measure
                });
            }
            return recipeIngredientModels;
        }
    }
}
