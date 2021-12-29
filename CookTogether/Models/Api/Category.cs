using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CookTogether.Models.Api
{
    public class Category
    {
        [JsonPropertyName("idCategory")]
        public int Id { get; set; }

        [JsonPropertyName("strCategory")]
        public string Name { get; set; }

        [JsonPropertyName("strCategoryThumb")]
        public Uri ThumbnailUrl { get; set; }

        [JsonPropertyName("strCategoryDescription")]
        public string Description { get; set; } 
    }
}
