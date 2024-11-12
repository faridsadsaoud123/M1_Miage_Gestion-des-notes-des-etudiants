
namespace UniversiteDomain.Exceptions.EtudiantDansParcoursExceptions;
[Serializable]

public class DuplicateInscripionException : Exception
{
    public DuplicateInscripionException() : base() { }
    public DuplicateInscripionException(string message) : base(message) { }
    public DuplicateInscripionException(string message, Exception inner) : base(message, inner) { }
}