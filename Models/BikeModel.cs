using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BikeBuster.Models
{
    public class BikeModel
    {
        [Key]
        [JsonPropertyName("identificador")]
        public string Id { get; set; }

        [JsonPropertyName("ano")]
        public int Year { get; set; }

        [JsonPropertyName("modelo")]
        public string Model { get; set; }

        [JsonPropertyName("placa")]
        public required string Plate { get; set; }
    }
}
