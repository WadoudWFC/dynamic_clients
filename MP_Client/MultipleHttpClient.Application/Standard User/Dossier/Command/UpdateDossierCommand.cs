using System.Text.Json.Serialization;
using MediatR;
using MutipleHttpClient.Domain;
using MutipleHttpClient.Domain.Shared.DTOs.Dossier;

namespace MultipleHttpClient.Application.Dossier.Command
{
    public record UpdateDossierCommand : IRequest<Result<DossierUpdateResult>>
    {
        public Guid DossierId { get; set; }
        public Guid StatusId { get; set; }
        public Guid DemandTypeId { get; set; }
        public Guid PartnerId { get; set; }
        public Guid NatureActivityId { get; set; }
        public Guid VilleId { get; set; }
        public Guid ArrondissementId { get; set; }
        public Guid TypeBienId { get; set; }
        public Guid VisibiliteId { get; set; }
        public Guid UserId { get; set; }
        [JsonIgnore] public int InternalLocalId { get; set; }
        public string AddressLocal { get; set; }
        public string Superficie { get; set; }
        [NonNegative]
        public int PresenceSanitaire { get; set; }
        [NonNegative]
        public decimal Prix { get; set; }
        public string Facade { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string HoraireOuverture { get; set; }
        public string JourOuverture { get; set; }
        public string AnneExperience { get; set; }
        public string CommentaireLocal { get; set; }
        public string Ice { get; set; }
        public string Rc { get; set; }
        public string AutreVille { get; set; }
        public string NomLocal { get; set; }
        public string NomAgWfcProche { get; set; }
        public string DistBurWfcProche { get; set; }
        public string DistAgWfcProche { get; set; }
        [NonNegative]
        public int Potentiel { get; set; }
        [NonNegative]
        public int Pack { get; set; }
        [NonNegative]
        public int FormeJuridique { get; set; }
        [NonNegative]
        public int RegimeImposi { get; set; }
        [NonNegative]
        public int ModeMandataire { get; set; }
        public string IdentificationFiscale { get; set; }
    }
}
