using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BikeBuster.Models
{
    public class RentalModel
    {
        [Key]
        [JsonPropertyName("identificador")]
        public string Id { get; set; }

        [JsonPropertyName("valor_diaria")]
        public decimal DailyRate { get; set; }

        [JsonPropertyName("entregador_id")]
        public string RiderId { get; set; }

        [JsonPropertyName("moto_id")]
        public string BikeId { get; set; }

        [JsonPropertyName("data_inicio")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("data_termino")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("data_previsao_termino")]
        public DateTime ExpectedEndDate { get; set; }

        [JsonPropertyName("data_devolucao")]
        public DateTime? ReturnDate { get; set; }
    }
}
