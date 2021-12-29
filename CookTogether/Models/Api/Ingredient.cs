using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CookTogether.Models.Api
{
    public class Ingredient
    {
        [JsonPropertyName("idIngredient")]
        public string Id { get; set; }
        [JsonPropertyName("strIngredient")]
        public string Name { get; set; }

        [JsonIgnore]
        public Uri ThumbnailUrl { get; set; }
    }

}
