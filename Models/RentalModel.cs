using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BikeBuster.Models
{
    public enum RentalPlan
    {
        SevenDays = 7,
        FifteenDays = 15,
        ThirtyDays = 30,
        FortyFiveDays = 45,
        FiftyDays = 50
    }

    public class RentalModel
    {
        [Key]
        [JsonPropertyName("identificador")]
        public string? Id { get; set; }

        [JsonPropertyName("plano")]
        public RentalPlan Plan { get; set; }

        [JsonPropertyName("valor_diaria")]
        public decimal DailyRate { get; set; }

        [JsonPropertyName("entregador_id")]
        public string UserId { get; set; }

        [JsonPropertyName("moto_id")]
        public string BikeId { get; set; }

        [JsonPropertyName("data_inicio")]
        public DateTime ContractStartDate { get; set; }

        [JsonPropertyName("data_termino")]
        public DateTime? ContractEndDate { get; set; }

        [JsonPropertyName("data_previsao_termino")]
        public DateTime ContractExpectedEndDate { get; set; }


    }
}
