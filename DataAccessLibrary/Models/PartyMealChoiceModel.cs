using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    class PartyMealChoiceModel
    {
        string UserId { get; set; }
        int PartyId { get; set; }
        int MealId { get; set; }
    }
}
