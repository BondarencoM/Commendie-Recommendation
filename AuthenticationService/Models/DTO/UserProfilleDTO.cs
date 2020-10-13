using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models.DTO
{
    class  UserProfilleDTO
    {
        public string Username { get; set; }

        public UserProfilleDTO(ApplicationUser user)
        {
            Username = user.UserName;
        }
    }
}
