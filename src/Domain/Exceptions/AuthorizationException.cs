namespace RendezVous.Domain.Exceptions;

public class AuthorizationException : Exception
{
    public AuthorizationException() : base() { }

    public AuthorizationException(string message) : base(message) { }

    public AuthorizationException(string message, Exception innerException)
        : base(message, innerException) { }
}