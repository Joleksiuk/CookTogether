using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CookTogether.Models.Api
{
    public class Meal
    {
        [JsonPropertyName("idMeal")]
        public string Id { get; set; }
        [JsonPropertyName("strMeal")]
        public string Name { get; set; }
        [JsonPropertyName("strCategory")]
        public string CategoryName { get; set; }
        [JsonPropertyName("strArea")]
        public string AreaName { get; set; }
        [JsonPropertyName("strInstructions")]
        public string Recipe { get; set; }
        [JsonPropertyName("strMealThumb")]
        public Uri ThumbnailUrl { get; set; }
        [JsonPropertyName("strTags")]
        public string Tags { get; set; }
        [JsonPropertyName("strYoutube")]
        public Uri YoutubeUrl { get; set; }

        public string strIngredient1 { get; set; }
        public string strIngredient2 { get; set; }
        public string strIngredient3 { get; set; }
        public string strIngredient4 { get; set; }
        public string strIngredient5 { get; set; }
        public string strIngredient6 { get; set; }
        public string strIngredient7 { get; set; }
        public string strIngredient8 { get; set; }
        public string strIngredient9 { get; set; }
        public string strIngredient10 { get; set; }
        public string strIngredient11 { get; set; }
        public string strIngredient12 { get; set; }
        public string strIngredient13 { get; set; }
        public string strIngredient14 { get; set; }
        public string strIngredient15 { get; set; }
        public string strIngredient16 { get; set; }
        public string strIngredient17 { get; set; }
        public string strIngredient18 { get; set; }
        public string strIngredient19 { get; set; }
        public string strIngredient20 { get; set; }

        public string strMeasure1 { get; set; }
        public string strMeasure2 { get; set; }
        public string strMeasure3 { get; set; }
        public string strMeasure4 { get; set; }
        public string strMeasure5 { get; set; }
        public string strMeasure6 { get; set; }
        public string strMeasure7 { get; set; }
        public string strMeasure8 { get; set; }
        public string strMeasure9 { get; set; }
        public string strMeasure10 { get; set; }
        public string strMeasure11 { get; set; }
        public string strMeasure12 { get; set; }
        public string strMeasure13 { get; set; }
        public string strMeasure14 { get; set; }
        public string strMeasure15 { get; set; }
        public string strMeasure16 { get; set; }
        public string strMeasure17 { get; set; }
        public string strMeasure18 { get; set; }
        public string strMeasure19 { get; set; }
        public string strMeasure20 { get; set; }

        public string strSource { get; set; }

        public List<string> GetIngredientList()
        {
            List<string> ingredientList = new();
            ingredientList.Add(strIngredient1);
            ingredientList.Add(strIngredient2);
            ingredientList.Add(strIngredient3);
            ingredientList.Add(strIngredient4);
            ingredientList.Add(strIngredient5);
            ingredientList.Add(strIngredient6);
            ingredientList.Add(strIngredient7);
            ingredientList.Add(strIngredient8);
            ingredientList.Add(strIngredient9);
            ingredientList.Add(strIngredient10);
            ingredientList.Add(strIngredient11);
            ingredientList.Add(strIngredient12);
            ingredientList.Add(strIngredient13);
            ingredientList.Add(strIngredient14);
            ingredientList.Add(strIngredient15);
            ingredientList.Add(strIngredient16);
            ingredientList.Add(strIngredient17);
            ingredientList.Add(strIngredient18);
            ingredientList.Add(strIngredient19);
            ingredientList.Add(strIngredient20);
            return ingredientList;
        }

        public List<string> GetMeasureList()
        {
            List<string> measureList = new();
            measureList.Add(strMeasure1);
            measureList.Add(strMeasure2);
            measureList.Add(strMeasure3);
            measureList.Add(strMeasure4);
            measureList.Add(strMeasure5);
            measureList.Add(strMeasure6);
            measureList.Add(strMeasure7);
            measureList.Add(strMeasure8);
            measureList.Add(strMeasure9);
            measureList.Add(strMeasure10);
            measureList.Add(strMeasure11);
            measureList.Add(strMeasure12);
            measureList.Add(strMeasure13);
            measureList.Add(strMeasure14);
            measureList.Add(strMeasure15);
            measureList.Add(strMeasure16);
            measureList.Add(strMeasure17);
            measureList.Add(strMeasure18);
            measureList.Add(strMeasure19);
            measureList.Add(strMeasure20);
            return measureList;
        } 
    }

}
