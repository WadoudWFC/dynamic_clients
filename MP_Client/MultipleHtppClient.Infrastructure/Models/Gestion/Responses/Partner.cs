using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.Models.Gestion.Responses
{
    public class Partner
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("id_typepartenaire")]
        public int? PartnerTypeId { get; set; }
        [JsonPropertyName("id_typepersonne")]
        public int? PersonTypeId { get; set; }
        [JsonPropertyName("id_ville")]
        public int? CityId { get; set; }
        [JsonPropertyName("id_region")]
        public int? RegionId { get; set; }
        [JsonPropertyName("Code")]
        public string? Code { get; set; }
        [JsonPropertyName("nom")]
        public string? LastName { get; set; }
        [JsonPropertyName("prenom")]
        public string? FirstName { get; set; }
        [JsonPropertyName("mail")]
        public string? Email { get; set; }
        [JsonPropertyName("telephone")]
        public string? Phone { get; set; }
        [JsonPropertyName("cin")]
        public string? CIN { get; set; }
        [JsonPropertyName("raison_sociale")]
        public string? CompanyName { get; set; }
        [JsonPropertyName("pid")]
        public string? PID { get; set; }
        [JsonPropertyName("ice")]
        public string? ICE { get; set; }
        [JsonPropertyName("rc")]
        public string? RC { get; set; }
        [JsonPropertyName("rib")]
        public string? RIB { get; set; }
        [JsonPropertyName("tp")]
        public string? TP { get; set; }
        [JsonPropertyName("identification_fiscale")]
        public string? FiscalIdentification { get; set; }
        [JsonPropertyName("forme_juridique")]
        public int LegalForm { get; set; }
        [JsonPropertyName("regime_imposi")]
        public int TaxRegime { get; set; }
        [JsonPropertyName("mode_mandataire")]
        public int AgentMode { get; set; }
        [JsonPropertyName("genre")]
        public bool Gender { get; set; }
        [JsonPropertyName("statut")]
        public string? Status { get; set; }
        [JsonPropertyName("date_created")]
        public DateTime DateCreated { get; set; }
        [JsonPropertyName("date_updated")]
        public DateTime? DateUpdated { get; set; }
        [JsonPropertyName("date_deleted")]
        public DateTime? DateDeleted { get; set; }
        [JsonPropertyName("user_created")]
        public int UserCreated { get; set; }
        [JsonPropertyName("user_updated")]
        public int? UserUpdated { get; set; }
        [JsonPropertyName("user_deleted")]
        public int? UserDeleted { get; set; }
        [JsonPropertyName("Dossier")]
        public List<object> Dossiers { get; set; } = new List<object>();
        [JsonPropertyName("id_villeNavigation")]
        public string? NavigationCityId { get; set; }
        [JsonPropertyName("id_regionNavigation")]
        public string? RegionNavigationId { get; set; }
        [JsonPropertyName("id_typepartenaireNavigation")]
        public string? PartnerNavigationTypeId { get; set; }
        [JsonPropertyName("id_typepersonneNavigation")]
        public string? PersonNavigationTypeId { get; set; }
    }
}
