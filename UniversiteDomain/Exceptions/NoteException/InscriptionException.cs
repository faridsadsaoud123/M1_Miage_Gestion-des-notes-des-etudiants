namespace UniversiteDomain.Exceptions.NoteException;

public class InscriptionException : Exception
{
    public InscriptionException() : base() { }
    public InscriptionException(string message) : base(message) { }
    public InscriptionException(string message, Exception inner) : base(message, inner) { }
}