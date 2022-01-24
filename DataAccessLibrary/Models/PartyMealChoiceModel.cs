using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class PartyMealChoiceModel
    {
        public string UserId { get; set; }
        public int PartyId { get; set; }
        public int MealId { get; set; }
    }
}
