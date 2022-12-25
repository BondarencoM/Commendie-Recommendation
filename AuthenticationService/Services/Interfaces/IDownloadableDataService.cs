using AuthenticationService.Data;
using AuthenticationService.Data.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationService.Services.Interfaces
{
    public interface IDownloadableDataService : IRabbitEventHandler
    {
        Task<List<DownloadablePersonalData>> GetData(string username);
    }
}
