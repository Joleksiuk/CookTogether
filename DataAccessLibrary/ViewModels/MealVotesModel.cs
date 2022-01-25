using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.ViewModels
{
    public class MealVotesModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
        public int CategoryId { get; set; }
        public int AreaId { get; set; }
        public int VoteNumber { get; set; }
        public int NumberUsersVoted { get; set; }
    }
}
