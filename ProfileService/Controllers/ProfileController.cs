using Microsoft.AspNetCore.Mvc;

namespace ProfileService.Profiles;

[ApiController]
[Route("api/profiles")]
public class ProfileController : ControllerBase
{
    private readonly ILogger<ProfileController> _logger;
    private readonly IProfileService profileService;

    public ProfileController(ILogger<ProfileController> logger, IProfileService profileService)
    {
        _logger = logger;
        this.profileService = profileService;
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<Profile>> GetProfile(string username)
    {
        try
        {
            return await profileService.FindByUsername(username);
        } 
        catch(ProfileNotFoundException)
        {
            return this.NotFound();
        }
    }
}