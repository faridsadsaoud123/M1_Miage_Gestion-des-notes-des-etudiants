
namespace UniversiteDomain.Exceptions.ParcoursException;
[Serializable]

public class ParcoursNotFoundException : Exception
{
    public ParcoursNotFoundException() : base() { }
    public ParcoursNotFoundException(string message) : base(message) { }
    public ParcoursNotFoundException(string message, Exception inner) : base(message, inner) { }
}