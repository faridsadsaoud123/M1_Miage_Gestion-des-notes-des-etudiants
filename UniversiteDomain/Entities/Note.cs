namespace UniversiteDomain.Entities;

public class Note
{
    public float valeur { get; set; }
    public Etudiant Etudiant { get; set; }
    public Ue Ue { get; set; }
    public long EtudiantId { get; set; }
    public long UeId { get; set; }

}