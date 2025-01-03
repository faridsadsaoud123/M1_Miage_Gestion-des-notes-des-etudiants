using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class UeRepository(UniversiteDbContext context) : Repository<Ue>(context), IUeRepository
{
    public async Task AffecterUeParcours(long idUe, long idParcours)
    {
        ArgumentNullException.ThrowIfNull(Context.Parcours);
        ArgumentNullException.ThrowIfNull(Context.Ues);
        
        Ue u = (await Context.Ues.FindAsync(idUe))!;
        if (Context.Parcours != null)
        {
            Parcours p = (await Context.Parcours.FindAsync(idParcours))!;
            if (u.EnseigneeDans != null && !u.EnseigneeDans.Contains(p))
            {
                u.EnseigneeDans?.Add(p);
            }
        }

        await Context.SaveChangesAsync();
    }

    public async Task AffecterUeParcours(Ue u, Parcours p)
    {
        await AffecterUeParcours(u.Id, p.Id);
    }
}