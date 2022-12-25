using Newtonsoft.Json.Linq;

namespace AuthenticationService.Data.Messages
{
    public class DownloadablePersonalDataIM
    {
        public string Username { get; set; }

        public string Domain{ get; set; }

        public JToken JsonData { get; set; }
    }
}
