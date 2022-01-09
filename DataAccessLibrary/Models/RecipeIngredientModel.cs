using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class RecipeIngredientModel
    {
        public int MealId { get; set; }
        public int IngredientId { get; set; }
        public string Measure { get; set; }
    }
}
