using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookTogether.Models.Api;
using DataAccessLibrary.MealRepositories;
using DataAccessLibrary.Models;

namespace CookTogether.Data
{
    public class DatabaseUpdateFromApiService
    {
        private readonly MealRepositories repositories;
        private readonly MealApiService mealApiService;

        public DatabaseUpdateFromApiService(MealApiService mealApiService, MealRepositories repositories)
        {
            this.repositories = repositories;
            this.mealApiService = mealApiService;
        }

        public async Task RunAll()
        {
            await Task.WhenAll(
                UpdateAreas(),
                UpdateCategories(),
                UpdateIngredients());
            await UpdateMeals();
        }

        public async Task UpdateAreas()
        {
            Area[] areas = await mealApiService.GetAreas();
            
            List<AreaModel> areaModels = new();
            foreach(Area area in areas)
            {
                areaModels.Add(new AreaModel()
                {
                    Name = area.Name
                });
            }
            await repositories.AreaRepository.InsertAreasIfNotExists(areaModels);
        }

        public async Task UpdateCategories()
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
            await repositories.CategoryRepository.InsertOrUpdateCategories(categoryModels);
        }

        public async Task UpdateIngredients()
        {
            Ingredient[] ingredients = await mealApiService.GetIngredients();
            List<IngredientModel> ingredientModels = new();
            foreach(Ingredient ingredient in ingredients)
            {
                ingredientModels.Add(new IngredientModel()
                {
                    Id = int.Parse(ingredient.Id),
                    Name = ingredient.Name,
                    ThumbnailSmallUrl = (ingredient.ThumbnailSmallUrl != null) ? ingredient.ThumbnailSmallUrl.ToString() : null
                });
            }
            await repositories.IngredientRepository.InsertOrUpdateIngredients(ingredientModels);
        }

        public async Task UpdateMeals()
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
                var categoryTask = repositories.CategoryRepository.GetCategoryByName(meal.CategoryName);
                var areaTask = repositories.AreaRepository.GetAreaByName(meal.AreaName);
                var recipeIngredientsTask = CreateIngredientModelsForMeal(meal);

                await Task.WhenAll(categoryTask, areaTask, recipeIngredientsTask);

                CategoryModel category = await categoryTask;
                AreaModel area = await areaTask;
                List<RecipeIngredientModel> recipeIngredients = await recipeIngredientsTask;

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

            await repositories.MealRepository.InsertOrUpdateMeals(mealModels);
            await repositories.MealRepository.InsertOrUpdateMealIngredients(recipeIngredientModels);      
        }

        private async Task<List<RecipeIngredientModel>> CreateIngredientModelsForMeal(Meal meal)
        {
            List<string> ingredientNames = meal.GetIngredientList();
            List<string> ingredientMeasures = meal.GetMeasureList();
            List<RecipeIngredientModel> recipeIngredientModels = new();


            ingredientNames.RemoveAll(name => name == String.Empty);
            ingredientMeasures.RemoveAll(measure => measure == String.Empty);

            List<IngredientModel> ingredientModels = await repositories.IngredientRepository.GetIngredientsByNames(ingredientNames);

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
