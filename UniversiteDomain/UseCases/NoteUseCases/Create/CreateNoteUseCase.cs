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
            EtudiantId = etudiantId,
            UeId = ueId
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
        var ue = await repositoryFactory.UeRepository().FindAsync(note.UeId);
        if (ue == null)
        {
            throw new EtudiantNotFoundException($"UE with ID {note.UeId} does not exist.");
        }

        // Vérifie si l'étudiant existe
        var etudiant = await repositoryFactory.EtudiantRepository().FindAsync(note.EtudiantId);
        if (etudiant == null)
        {
            throw new UeNotFoundException($"Student with ID {note.EtudiantId} does not exist.");
        }

        // Vérifie si l'étudiant est inscrit au parcours contenant l'UE
        if (etudiant.ParcoursSuivi == null && etudiant.ParcoursSuivi.UesEnseignees.All(u=>u.Id!=note.UeId))
        {
            throw new InscriptionException($"Student with ID {note.EtudiantId} n'est pas inscrit à l'UE {note.UeId}.");
        }

        
        

        // Vérifie si l'étudiant a déjà une note pour cette UE
        var existingNote = await repositoryFactory.NoteRepository()
            .FindByConditionAsync(n => n.EtudiantId == note.EtudiantId && n.UeId == note.UeId);
        if (existingNote != null)
        {
            throw new AlreadyHaveNoteException($"Student with ID {note.EtudiantId} already has a note for UE {note.UeId}.");
        }
    }

}
