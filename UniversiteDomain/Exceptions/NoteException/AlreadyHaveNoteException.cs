namespace UniversiteDomain.Exceptions.NoteException;

public class AlreadyHaveNoteException : Exception
{
    public AlreadyHaveNoteException() : base() { }
    public AlreadyHaveNoteException(string message) : base(message) { }
    public AlreadyHaveNoteException(string message, Exception inner) : base(message, inner) { }
}