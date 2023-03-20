namespace MaratukAdmin.Exceptions;

public class PermissionDeniedException : ApplicationException
{
    public PermissionDeniedException(string message) : base(message)
    {
    }
}