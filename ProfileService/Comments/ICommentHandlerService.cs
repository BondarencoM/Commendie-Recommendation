using ProfileService.Common;

namespace ProfileService.Comments;

public interface ICommentService : IRabbitEventHandler
{
    Task<List<Comment>> GetCommentsForUser(string username, int limit, int skip);
}
