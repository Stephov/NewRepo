namespace MaratukAdmin.Exceptions;

public class ResourceNotFoundException : ApplicationException
{
    public ResourceNotFoundException(string message) : base(message)
    {
    }
}