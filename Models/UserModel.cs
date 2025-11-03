using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace BikeBuster.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DriverLicenseType
    {
        A,
        B,
        AB
    }

    [Index(nameof(Cnpj), IsUnique = true)]
    [Index(nameof(DriverLicenseNumber), IsUnique = true)]
    public class UserModel
    {
        [Key]
        [JsonPropertyName("identificador")]
        public required string Id { get; set; }

        [JsonPropertyName("nome")]
        [Required]
        [StringLength(200)]
        public required string Name { get; set; }

        [JsonPropertyName("data_nascimento")]
        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [JsonPropertyName("tipo_cnh")]
        public DriverLicenseType DriverLicenseType { get; set; }

        [Required]
        [JsonPropertyName("imagem_cnh")]
        public required string DriverLicenseImage { get; set; }

        [JsonPropertyName("numero_cnh")]
        [Required]
        [StringLength(11, MinimumLength = 11)]
        public required string DriverLicenseNumber { get; set; }

        [JsonPropertyName("cnpj")]
        [Required]
        [StringLength(14, MinimumLength = 14)]
        public required string Cnpj { get; set; }
    }
}