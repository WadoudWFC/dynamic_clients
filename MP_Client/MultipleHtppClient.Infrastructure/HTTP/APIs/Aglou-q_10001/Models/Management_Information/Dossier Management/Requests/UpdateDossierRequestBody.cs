using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Dossier_Management.Requests;

public class UpdateDossierRequestBody
{
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }
    [JsonPropertyName("id_statutdossier")]
    public int IdStatutDossier { get; set; }
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("localid")]
    public int LocalId { get; set; }
    [JsonPropertyName("id_typedemende")]
    public int IdTypeDemende { get; set; }
    [JsonPropertyName("id_partenaire")]
    public int IdPartenaire { get; set; }
    [JsonPropertyName("id_natureactivite")]
    public int IdNatureActivite { get; set; }
    [JsonPropertyName("id_ville")]
    public int IdVille { get; set; }
    [JsonPropertyName("id_arrandissement")]
    public int IdArrondissement { get; set; }
    [JsonPropertyName("adresslocal")]
    public string? AddressLocal { get; set; }
    [JsonPropertyName("id_visibilite")]
    public int IdVisibilite { get; set; }
    [JsonPropertyName("superficie")]
    public string? Superficie { get; set; }
    [JsonPropertyName("presence_sanitaire")]
    public int PresenceSanitaire { get; set; }
    [JsonPropertyName("id_typebien")]
    public int IdTypeBien { get; set; }
    [JsonPropertyName("prix")]
    public decimal Prix { get; set; }
    [JsonPropertyName("facade")]
    public string? Facade { get; set; }
    [JsonPropertyName("Latitude")]
    public string? Latitude { get; set; }
    [JsonPropertyName("Longitude")]
    public string? Longitude { get; set; }
    [JsonPropertyName("horaire_ouverture")]
    public string? HoraireOuverture { get; set; }
    [JsonPropertyName("jour_ouverture")]
    public string? JourOuverture { get; set; }
    [JsonPropertyName("anne_experience")]
    public string? AnneExperience { get; set; }
    [JsonPropertyName("commentairelocal")]
    public string? CommentaireLocal { get; set; }
    [JsonPropertyName("ice")]
    public string? Ice { get; set; }
    [JsonPropertyName("rc")]
    public string? Rc { get; set; }
    [JsonPropertyName("autre_ville")]
    public string? AutreVille { get; set; }
    [JsonPropertyName("nomlocal")]
    public string? NomLocal { get; set; }
    [JsonPropertyName("nom_ag_WFC_proche")]
    public string? NomAgWfcProche { get; set; }
    [JsonPropertyName("dist_bur_WFC_proche")]
    public string? DistBurWfcProche { get; set; }
    [JsonPropertyName("dist_ag_WFC_proche")]
    public string? DistAgWfcProche { get; set; }
    [JsonPropertyName("potentiel")]
    public int Potentiel { get; set; }
    [JsonPropertyName("pack")]
    public int Pack { get; set; }
    [JsonPropertyName("forme_juridique")]
    public int FormeJuridique { get; set; }
    [JsonPropertyName("regime_imposi")]
    public int RegimeImposi { get; set; }
    [JsonPropertyName("mode_mandataire")]
    public int ModeMandataire { get; set; }
    [JsonPropertyName("identification_fiscale")]
    public string? IdentificationFiscale { get; set; }
}
