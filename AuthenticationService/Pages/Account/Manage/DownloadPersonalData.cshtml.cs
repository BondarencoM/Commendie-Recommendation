using AuthenticationService.Data;
using AuthenticationService.Data.Messages;
using AuthenticationService.Services.Interfaces;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthenticationService.Areas.Identity.Pages.Account.Manage
{
    public class DownloadPersonalDataModel : PageModel
    {
        private readonly IUserPublisher userPublisher;
        private readonly IDownloadableDataService downloadableService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DownloadPersonalDataModel> _logger;

        public DownloadPersonalDataModel(
            IUserPublisher userPublisher,
            IDownloadableDataService downloadableService,
            UserManager<ApplicationUser> userManager,
            ILogger<DownloadPersonalDataModel> logger)
        {
            this.userPublisher = userPublisher;
            this.downloadableService = downloadableService;
            _userManager = userManager;
            _logger = logger;
        }

        public List<DownloadablePersonalData> Downloadables { get; set; }

        public async Task<IActionResult> OnGet()
        {
            this.Downloadables = await downloadableService.GetData(User.GetDisplayName());

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            await this.userPublisher.DataRequested(new UserIdentifierMessage
            {
                Username = this.User.GetDisplayName(),
            });

            return await OnGet();
        }

        public async Task<IActionResult> OnPostDownloadAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

            var otherData = await downloadableService.GetData(user.UserName);

            var allData = otherData.ToDictionary(
                d => d.DomainName,
                d => JsonSerializer.Deserialize<ExpandoObject>(d.JsonData) as object
            );

            allData["authentication"] = await this.GetAuthenticationUserData(user);
            
            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(allData), "application/json");
        }

        private async Task<Dictionary<string,string>> GetAuthenticationUserData(ApplicationUser user)
        {
            // Only include personal data for download
            var personalData = new Dictionary<string, string>();
            var personalDataProps = typeof(ApplicationUser).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            var logins = await _userManager.GetLoginsAsync(user);
            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            return personalData;
        }
    }
}
