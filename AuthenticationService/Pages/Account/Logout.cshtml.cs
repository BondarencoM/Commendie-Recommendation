using AuthenticationService.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AuthenticationService.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;

        public LogoutModel(
            SignInManager<ApplicationUser> signInManager,
            ILogger<LogoutModel> logger,
            IIdentityServerInteractionService interaction,
            IEventService events)
        {
            _signInManager = signInManager;
            _logger = logger;
            _interaction = interaction;
            _events = events;
        }

        public bool AutoLogoutAndRedirect { get; set; } = false;

        public async Task<IActionResult> OnGet(string logoutId = null)
        {
            if (User?.Identity.IsAuthenticated != true)
                return LocalRedirect("/");

            var context = await _interaction.GetLogoutContextAsync(logoutId);

            //it it's safe to logout directly
            if (context?.ShowSignoutPrompt == false)
                return await OnPost(logoutId);

            return Page();
        }

        public async Task<IActionResult> OnPost(string logoutId = null)
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");
            }

            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            if (!string.IsNullOrWhiteSpace(logout.PostLogoutRedirectUri))
                return Redirect(logout.PostLogoutRedirectUri);

            return LocalRedirect("/");
        }
    }
}
