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
        [Range(1900, 2100)]
        public int Year { get; set; }

        [JsonPropertyName("modelo")]
        public string Model { get; set; }

        [JsonPropertyName("placa")]
        [StringLength(7, MinimumLength = 7)]
        public required string Plate { get; set; }
    }
}
