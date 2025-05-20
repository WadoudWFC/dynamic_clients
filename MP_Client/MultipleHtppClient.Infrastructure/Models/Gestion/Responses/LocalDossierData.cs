using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure;

public class LocalDossierData
{
    [JsonPropertyName("id_dossier")]
    public int? DossierId { get; set; }
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("photosInterieur")]
    public object? InteriorPhotos { get; set; }
    [JsonPropertyName("photosExterieur")]
    public object? ExteriorPhotos { get; set; }
    [JsonPropertyName("Latitude")]
    public string? Latitude { get; set; }
    [JsonPropertyName("Longitude")]
    public string? Longitude { get; set; }
    [JsonPropertyName("adress")]
    public string? Address { get; set; }
    [JsonPropertyName("id_ville")]
    public int? CityId { get; set; }
    [JsonPropertyName("id_arrandissement")]
    public int? DistrictId { get; set; }
    [JsonPropertyName("commentaireLocal")]
    public string? LocalComment { get; set; }
    [JsonPropertyName("id_typebien")]
    public int? PropertyTypeId { get; set; }
    [JsonPropertyName("adresslocal")]
    public string? LocalAddress { get; set; }
    [JsonPropertyName("horaire_ouverture")]
    public string? OpeningHours { get; set; }
    [JsonPropertyName("jour_ouverture")]
    public string? OpeningDays { get; set; }
    [JsonPropertyName("id_visibilite")]
    public int? VisibilityId { get; set; }
    [JsonPropertyName("superficie")]
    public string? Area { get; set; }
    [JsonPropertyName("presence_sanitaire")]
    public int? HasSanitary { get; set; }
    [JsonPropertyName("typebien")]
    public string? PropertyType { get; set; }
    [JsonPropertyName("facade")]
    public string? Facade { get; set; }
    [JsonPropertyName("anne_experience")]
    public string? YearsOfExperience { get; set; }
    [JsonPropertyName("prix")]
    public decimal? Price { get; set; }
    [JsonPropertyName("zon")]
    public string? Zone { get; set; }
    [JsonPropertyName("ville")]
    public string? City { get; set; }
    [JsonPropertyName("region")]
    public string? Region { get; set; }
    [JsonPropertyName("arrondissement")]
    public string? District { get; set; }
    [JsonPropertyName("typelocalite")]
    public string? LocalityType { get; set; }
    [JsonPropertyName("decopagecmr")]
    public string? CmrDecoration { get; set; }
    [JsonPropertyName("nom_ag_WFC_proche")]
    public string? NearestWFCAgencyName { get; set; }
    [JsonPropertyName("dist_bur_WFC_proche")]
    public string? NearestWFCOfficeDistance { get; set; }
    [JsonPropertyName("potentiel")]
    public int? Potential { get; set; }
    [JsonPropertyName("dist_ag_WFC_proche")]
    public string? NearestWFCAgencyDistance { get; set; }
    [JsonPropertyName("pack")]
    public string? Pack { get; set; }
    [JsonPropertyName("id_pack")]
    public int? PackId { get; set; }
    [JsonPropertyName("nom")]
    public string? Name { get; set; }
}
