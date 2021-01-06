using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace AuthenticationService.Pages
{
    public class IndexModel : PageModel
    {
        public string LinkToHome { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            LinkToHome = configuration.GetSection("Clients:Cors").GetChildren().Select(s => s.Value).FirstOrDefault();
        }

        public void OnGet()
        {
        }
    }
}
