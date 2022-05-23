using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CookTogether.Models.Api
{
    public class MealApiService
    {
        private static readonly string API_KEY = "1"; //for development purposes  
        private static readonly string URL_PREFIX = $"https://www.themealdb.com/api/json/v1/{API_KEY}/";

        // c - for categories, i - for ingredients, a - for areas
        private static readonly string LIST_URL_FORMAT = "list.php?{0}=list"; 
        private static readonly string AREAS_URL = URL_PREFIX + String.Format(LIST_URL_FORMAT, "a");
        private static readonly string INGREDIENTS_URL = URL_PREFIX + String.Format(LIST_URL_FORMAT, "i");

        private static readonly string CATEGORIES_DETAILS_URL = URL_PREFIX + "categories.php";

        private static readonly string MEALS_BY_STARTS_WITH_URL_FORMAT = URL_PREFIX + "search.php?f={0}"; //STARTING STRING

        private static readonly string INGREDIENT_THUMBNAIL_URL_SMALL_FORMAT = "https://www.themealdb.com/images/ingredients/{0}-Small.png"; //INGREDIENT_NAME

        private readonly HttpClient httpClient;

        public MealApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Category[]> GetCategories()
        {
            Category[] categories = Array.Empty<Category>();

            try
            {
                CategoriesResponse response = await httpClient.GetFromJsonAsync<CategoriesResponse>(CATEGORIES_DETAILS_URL);
                if (response.Categories != null)
                {
                    categories = response.Categories;
                }
            }
            catch(HttpRequestException e) { }
            
            return categories; 
        }

        public async Task<string> GetCategoriesListAsString()
        {
            string response;
            try
            {
                response = await httpClient.GetStringAsync(CATEGORIES_DETAILS_URL);
            }
            catch(HttpRequestException e)
            {
                response = e.Message;
            }
            return response;
        }

        public async Task<Area[]> GetAreas()
        {
            Area[] areas = Array.Empty<Area>();
            try
            {
                AreasResponse response = await httpClient.GetFromJsonAsync<AreasResponse>(AREAS_URL);
                if (response.Areas != null)
                {
                    areas = response.Areas;
                }
            }
            catch(HttpRequestException e) { }
            return areas;
        }

        public async Task<Ingredient[]> GetIngredients()
        {
            Ingredient[] ingredients = Array.Empty<Ingredient>();
            try
            {
                IngredientsResponse response = await httpClient.GetFromJsonAsync<IngredientsResponse>(INGREDIENTS_URL);
                if (response.Ingredients != null)
                {
                    ingredients = response.Ingredients;
                    foreach(var ingredient in ingredients)
                    {
                        Uri thumbnailUrl = new Uri(Uri.EscapeUriString(String.Format(INGREDIENT_THUMBNAIL_URL_SMALL_FORMAT, ingredient.Name)));
                        if (await ImageExists(thumbnailUrl))
                        {
                            ingredient.ThumbnailSmallUrl = thumbnailUrl;
                        }
                    }
                }
            }
            catch(HttpRequestException e) { }
            return ingredients;
        }

        private async Task<bool> ImageExists(Uri imageUri)
        {
            var get = await httpClient.GetAsync(imageUri);
            return get.IsSuccessStatusCode;
        }

        public async Task<Meal[]> GetMealsStartingWith(string starting)
        {
            Meal[] meals = Array.Empty<Meal>();
            string mealsUrl = String.Format(MEALS_BY_STARTS_WITH_URL_FORMAT, starting);
            try
            {
                MealsResponse response = await httpClient.GetFromJsonAsync<MealsResponse>(mealsUrl);
                if (response.Meals != null)
                {
                    meals = response.Meals;
                }
            }
            catch(HttpRequestException e) { }
            return meals;
        }
    }
}
