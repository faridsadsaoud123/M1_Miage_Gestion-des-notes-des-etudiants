namespace UniversiteDomain.Exceptions.NoteException;
[Serializable]
public class NoteValueException : Exception
{
    public NoteValueException() : base() { }
    public NoteValueException(string message) : base(message) { }
    public NoteValueException(string message, Exception inner) : base(message, inner) { }
}