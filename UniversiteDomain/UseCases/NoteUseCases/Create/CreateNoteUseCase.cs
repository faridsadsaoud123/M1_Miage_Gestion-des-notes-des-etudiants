using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;
using UniversiteDomain.Exceptions.NoteException;
using UniversiteDomain.Exceptions.UeExceptions;

namespace UniversiteDomain.UseCases.NoteUseCases.Create;

public class CreateNoteUseCase
{
    private readonly IRepositoryFactory repositoryFactory;

    public CreateNoteUseCase(IRepositoryFactory repositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(repositoryFactory);
        this.repositoryFactory = repositoryFactory;
    }

    public async Task<Note> ExecuteAsync(float val, long etudiantId, long ueId)
    {
        var note = new Note
        {
            valeur = val,
            etud = etudiantId,
            u = ueId
        };
        return await ExecuteAsync(note);
    }

    public async Task<Note> ExecuteAsync(Note note)
    {
        await CheckBusinessRules(note);
        Note createdNote = await repositoryFactory.NoteRepository().CreateAsync(note);
        await repositoryFactory.NoteRepository().SaveChangesAsync();
        return createdNote;
    }

    private async Task CheckBusinessRules(Note note)
    {
        ArgumentNullException.ThrowIfNull(note);
        ArgumentNullException.ThrowIfNull(note.valeur);

        // Vérifie si la note est comprise entre 0 et 20
        if (note.valeur < 0 || note.valeur > 20)
        {
            throw new NoteValueException($"{note.valeur} is out of range.");
        }

        // Vérifie si l'UE existe
        var ue = await repositoryFactory.UeRepository().FindAsync(note.u);
        if (ue == null)
        {
            throw new EtudiantNotFoundException($"UE with ID {note.u} does not exist.");
        }

        // Vérifie si l'étudiant existe
        var etudiant = await repositoryFactory.EtudiantRepository().FindAsync(note.etud);
        if (etudiant == null)
        {
            throw new UeNotFoundException($"Student with ID {note.etud} does not exist.");
        }

        // Vérifie si l'étudiant est inscrit au parcours contenant l'UE
        if (etudiant.ParcoursSuivi == null && etudiant.ParcoursSuivi.UesEnseignees.All(u=>u.Id!=note.u))
        {
            throw new InscriptionException($"Student with ID {note.etud} n'est pas inscrit à l'UE {note.u}.");
        }

        
        

        // Vérifie si l'étudiant a déjà une note pour cette UE
        var existingNote = await repositoryFactory.NoteRepository()
            .FindByConditionAsync(n => n.etud == note.etud && n.u == note.u);
        if (existingNote != null)
        {
            throw new AlreadyHaveNoteException($"Student with ID {note.etud} already has a note for UE {note.u}.");
        }
    }

}
