namespace UniversiteDomain.Exceptions.ParcoursException;

[Serializable]
public class DuplicateParcoursException : Exception
{
    public DuplicateParcoursException() : base() { }
    public DuplicateParcoursException(string message) : base(message) { }
    public DuplicateParcoursException(string message, Exception inner) : base(message, inner) { }
}