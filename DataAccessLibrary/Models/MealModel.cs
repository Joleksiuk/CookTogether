using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class MealModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Recipe { get; set; }
        public string ThumbnailUrl { get; set; }
        public int CategoryId { get; set; }
        public int AreaId { get; set; }
    }
}
