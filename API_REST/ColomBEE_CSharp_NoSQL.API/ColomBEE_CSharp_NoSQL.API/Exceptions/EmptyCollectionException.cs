namespace ColomBEE_CSharp_NoSQL.API.Exceptions;

public class EmptyCollectionException(string message) : Exception(message)
{
}