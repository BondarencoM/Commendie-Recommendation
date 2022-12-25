using AuthenticationService.Data;

namespace AuthenticationService.Data.DTO
{
    class UserProfilleDTO
    {
        public string Username { get; set; }

        public UserProfilleDTO(ApplicationUser user)
        {
            Username = user.UserName;
        }
    }
}
