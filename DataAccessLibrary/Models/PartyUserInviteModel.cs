using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class PartyUserInviteModel
    {
        public string InvitedUserId { get; set; }
        public int PartyId { get; set; }
    }
}
