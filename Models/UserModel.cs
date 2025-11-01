using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace BikeBuster.Models
{
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
        public string Id { get; set; }

        [JsonPropertyName("nome")]
        public string Name { get; set; }

        [JsonPropertyName("data_nascimento")]
        public DateTime BirthDate { get; set; }

        [JsonPropertyName("tipo_cnh")]
        public DriverLicenseType DriverLicenseType { get; set; }

        [JsonPropertyName("imagem_cnh")]
        public string DriverLicenseImage { get; set; }

        [JsonPropertyName("numero_cnh")]
        [Required]
        [StringLength(11, MinimumLength = 11)]
        public uint DriverLicenseNumber { get; set; }

        [JsonPropertyName("cnpj")]
        [Required]
        [StringLength(14, MinimumLength = 14)]
        public uint Cnpj { get; set; }
    }
}
