using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CookTogether.Models.Api
{
    public class AreasResponse
    {
        [JsonPropertyName("meals")]
        public Area[] Areas { get; set; }
    }
}
