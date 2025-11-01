using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;


namespace BikeBuster.Models
{
    [Index(nameof(Plate), IsUnique = true)]
    public class BikeModel
    {
        [Key]
        [JsonPropertyName("identificador")]
        public string Id { get; set; }

        [JsonPropertyName("ano")]
        [StringLength(4, MinimumLength = 4)]
        public int Year { get; set; }

        [JsonPropertyName("modelo")]
        public string Model { get; set; }

        [JsonPropertyName("placa")]
        public required string Plate { get; set; }
    }
}
