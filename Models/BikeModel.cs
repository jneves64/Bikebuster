using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;


namespace BikeBuster.Models
{
    [Index(nameof(Plate), IsUnique = true)]
    public class BikeModel
    {
        [Key]
        [JsonPropertyName("identificador")]
        [Required]
        public required string Id { get; set; }

        [JsonPropertyName("ano")]
        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; }

        [JsonPropertyName("modelo")]
        [Required]
        public required string Model { get; set; }

        [JsonPropertyName("placa")]
        [Required]
        [StringLength(7, MinimumLength = 7)]
        public string Plate
        {
            get => _plate;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                _plate = Regex.Replace(value.ToUpper(), "[^A-Z0-9]", "");
            }
        }
        private string _plate = null!;

    }
}
