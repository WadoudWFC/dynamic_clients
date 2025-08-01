﻿using System.Text.Json.Serialization;

namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.Management_Information.Dossier_Management.Responses
{
    public class DossierAll
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("Code")]
        public string? Code { get; set; }
        [JsonPropertyName("id_typedemende")]
        public int DemandTypeId { get; set; }
        [JsonPropertyName("id_statutdossier")]
        public int DossierStatusId { get; set; }
        [JsonPropertyName("statut")]
        public string? Status { get; set; }
    }
}
