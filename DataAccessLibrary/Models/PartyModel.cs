using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    class PartyModel
    {
        int Id { get; set; }
        DateTime CreationDate { get; set; }
        int CategoryId { get; set; }
        int AreaId { get; set; }
        string OwnerUserId { get; set; }
        string Name { get; set; }
    }
}
