using Microsoft.EntityFrameworkCore;
using ProfileService.Comments;

namespace ProfileService.Profiles;

[PrimaryKey(nameof(Id))]
public class Profile
{
    public Profile()
    {
    }

    public Profile(CreateProfileIM input)
    {
        this.Id = input.Username;
    }

    public string Id { get; set; } = null!;

    public List<Comment> Comments { get; set; } = null!;

}
