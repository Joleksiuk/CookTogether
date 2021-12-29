using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CookTogether.Models
{
    public class DisplayUserModel
    {

        public string id { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Username is too long")]
        [MinLength(5, ErrorMessage = "Username is too short")]
        public string username { get; set; }
        public string password_hash { get; set; }
    }
}
