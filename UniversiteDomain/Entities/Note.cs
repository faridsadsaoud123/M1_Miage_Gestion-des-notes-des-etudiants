namespace UniversiteDomain.Entities;

public class Note
{
    public float valeur { get; set; }
    public Etudiant etudiant { get; set; }
    public Ue ue { get; set; }
    public long etud { get; set; }
    public long u { get; set; }

}