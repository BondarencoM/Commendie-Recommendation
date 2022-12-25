using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AuthenticationService.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public IEnumerable<DownloadablePersonalData> DownloadablePersonalDatas { get; set; }
    }
}
