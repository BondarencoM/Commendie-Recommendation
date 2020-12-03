using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Models
{
    public class UserProfile
    {
        [Key]
        public long Id { get; set; }
        public string Username { get; set; }
        
    }
}
