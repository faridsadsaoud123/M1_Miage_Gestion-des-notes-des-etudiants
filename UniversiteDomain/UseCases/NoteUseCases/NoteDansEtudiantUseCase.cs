using System;
using System.Linq;
using System.Threading.Tasks;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.NoteUseCases
{
    public class NoteDansEtudiantUseCase(IRepositoryFactory repositoryFactory)
    {
        

        public async Task<Note> ExecuteAsync(long etudiantId, long ueId, float valeurNote)
        {
            if (valeurNote < 0 || valeurNote > 20)
                throw new ArgumentOutOfRangeException(nameof(valeurNote), "La note doit être comprise entre 0 et 20.");

            // Vérification de l'existence de l'étudiant
            var etudiantRepository = repositoryFactory.EtudiantRepository();
            var etudiants = await etudiantRepository.FindByConditionAsync(e => e.Id == etudiantId);
            var etudiant = etudiants.FirstOrDefault();

            if (etudiant == null)
                throw new InvalidOperationException($"Aucun étudiant trouvé avec l'ID {etudiantId}.");

            // Vérification de l'existence de l'UE
            var ueRepository = repositoryFactory.UeRepository();
            var ues = await ueRepository.FindByConditionAsync(u => u.Id == ueId);
            var ue = ues.FirstOrDefault();

            if (ue == null)
                throw new InvalidOperationException($"Aucune UE trouvée avec l'ID {ueId}.");

            // Création de la note
            var noteRepository = repositoryFactory.NoteRepository();
            var nouvelleNote = new Note
            {
                valeur = valeurNote,
                etud = etudiantId,
                u = ueId
            };

            var noteCreee = await noteRepository.CreateAsync(nouvelleNote);

            // Mise à jour de la liste des notes de l'étudiant
            etudiant.Notes ??= new List<Note>();
            etudiant.Notes.Add(noteCreee);

            // Mise à jour de l'entité Étudiant dans le repository
            await etudiantRepository.UpdateAsync(etudiant);

            return noteCreee;
        }
    }
}
