namespace ShipParticularsApi.Exceptions
{
    public class DatabaseConstraintException(string message, Exception exception)
        : ApplicationException(message, exception)
    {
    }
}
