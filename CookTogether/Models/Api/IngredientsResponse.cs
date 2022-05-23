using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CookTogether.Models.Api
{
    public class IngredientsResponse
    {
        [JsonPropertyName("meals")]
        public Ingredient[] Ingredients { get; set; }
    }
}
