using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.NoteUseCases.Create;

namespace UniversiteDomainUnitTests
{
    public class NoteUnitTests
    {
        private Mock<INoteRepository> mockNoteRepository;
        private Mock<IRepositoryFactory> mockFactory;

        [SetUp]
        public void Setup()
        {
            mockNoteRepository = new Mock<INoteRepository>();
            mockFactory = new Mock<IRepositoryFactory>();
        }

        [Test]
        public async Task CreateNoteUseCase()
        {
           
            float valeurNote = (float)15.5;
            long etudiantId = 123;
            long ueId = 456;

            // Création des objets pour le test
            Note noteAvant = new Note
            {
                valeur = valeurNote,
                EtudiantId = etudiantId,
                UeId = ueId
            };

            Note noteFinale = new Note
            {
                valeur = valeurNote,
                EtudiantId = etudiantId,
                UeId = ueId
            };

            // Configuration des mocks
            mockNoteRepository
                .Setup(repo => repo.CreateAsync(noteAvant))
                .ReturnsAsync(noteFinale);

            mockFactory
                .Setup(factory => factory.NoteRepository())
                .Returns(mockNoteRepository.Object);

            var useCase = new CreateNoteUseCase(mockFactory.Object);

            // Act
            var noteCree = await useCase.ExecuteAsync(noteAvant);


            Assert.That(noteCree.valeur, Is.EqualTo(noteFinale.valeur));
            Assert.That(noteCree.EtudiantId, Is.EqualTo(noteFinale.EtudiantId));
            Assert.That(noteCree.UeId, Is.EqualTo(noteFinale.UeId));
        }
    }
}
