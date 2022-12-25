using CommentService.Models;

namespace CommentService.Services.Interfaces;

public interface IHasDownloadableUserData
{
    public Task<PersonalDataModel> GetDownloadableUserData(string username);
}
