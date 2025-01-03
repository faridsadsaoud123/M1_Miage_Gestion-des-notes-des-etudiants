using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class ParcoursRepository(UniversiteDbContext context) : Repository<Parcours>(context), IParcoursRepository
{
    public async Task<Parcours> AddEtudiantAsync(long idParcours, long idEtudiant)
    {
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        Etudiant e =(await Context.Etudiants.FindAsync(idEtudiant))!;

        if (!p.Inscrits.Contains(e))
        {
            p.Inscrits.Add(e);
        }
        await Context.SaveChangesAsync();
        return p;
    }
    public async Task<Parcours> AddEtudiantAsync(Parcours parcours, Etudiant etudiant)
    {
        Parcours p  = await AddEtudiantAsync(parcours.Id, etudiant.Id);
        return p;
    }



    public async Task<Parcours> AddEtudiantAsync(long idParcours, long[] idEtudiants)
    {
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        List<Etudiant> etudiants = new List<Etudiant>();
        for (int i = 0; i < idEtudiants.Length; i++)
        {
            Etudiant e = (await Context.Etudiants.FindAsync(idEtudiants[i]))!;
            etudiants.Add(e);
        }
        p.Inscrits?.AddRange(etudiants);
        await Context.SaveChangesAsync();
        return p;
    }
    public async Task<Parcours> AddEtudiantAsync(Parcours parcours, List<Etudiant> etudiants)
    {
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(etudiants);
        ArgumentNullException.ThrowIfNull(parcours);
        
        long[] idEtudiants = new long[]{};
        for (int i = 0; i < etudiants.Count; i++)
        {
            idEtudiants[i] = etudiants[i].Id;
        }
        Parcours p = await AddEtudiantAsync(parcours.Id, idEtudiants);
        return p;
    }
    public async Task<Parcours> AddUeAsync(long idParcours, long idUe)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        Ue u = (await Context.Ues.FindAsync(idUe))!;
        if (!p.UesEnseignees.Contains(u))
        {
            p.UesEnseignees.Add(u);
        }
        await Context.SaveChangesAsync();
        return p;
    }

    public async Task<Parcours> AddUeAsync(long idParcours, long[] idUes)
    {
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        
        Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
        List<Ue> ues = new List<Ue>();
        for (int i = 0; i < idUes.Length; i++)
        {
            Ue u = (await Context.Ues.FindAsync(idUes[i]))!;
            ues.Add(u);
        }
        p.UesEnseignees.AddRange(ues);
        await Context.SaveChangesAsync();
        return p;
    }
}