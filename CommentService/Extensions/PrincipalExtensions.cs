using System.Security.Principal;

namespace CommentService.Extensions;

public static class PrincipalExtensions
{
    public static bool IsAdmin(this IPrincipal principal) => principal.IsInRole("admin");
}
