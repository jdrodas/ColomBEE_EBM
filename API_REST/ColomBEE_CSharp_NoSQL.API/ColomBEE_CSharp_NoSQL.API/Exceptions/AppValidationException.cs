namespace ColomBEE_CSharp_NoSQL.API.Exceptions;

public class AppValidationException(string message) : Exception(message)
{
}