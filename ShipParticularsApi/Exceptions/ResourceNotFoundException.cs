namespace ShipParticularsApi.Exceptions
{
    public class ResourceNotFoundException(string message)
        : ApplicationException(message)
    {
    }
}
