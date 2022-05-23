using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class PartyModel
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string OwnerUserId { get; set; }
        public string PartyName { get; set; }
    }
}
