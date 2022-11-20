using System.Security.Principal;

namespace CommentService.Exceptions;

public class CommentNotFoundException : Exception {}

/// <summary>
/// Thrown when a user tries to execute an operation but is not allowed for Authorization reasons
/// </summary>
public class OperationNotPermittedException : Exception
{
    /// <summary>
    /// Creates a new isntance of <see cref="OperationNotPermittedException"/>
    /// </summary>
    /// <param name="user">The user that tried to execute an operation.</param>
    /// <param name="operation">The operation that was not permited (e.g. add, edit, delete)</param>
    /// <param name="object">The object that the operation was supposed to affect</param>
    public OperationNotPermittedException(IPrincipal user, string operation, string @object)
        : base($"User {user.Identity?.Name} tried to {operation} {@object} but was not allowed")
    {
    }
}
