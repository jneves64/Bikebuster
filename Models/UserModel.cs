using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BikeBuster.Models
{
    public class UserModel
    {
        [Key]
        [JsonPropertyName("identificador")]
        public string Id { get; set; }

        [JsonPropertyName("nome")]
        public string Name { get; set; }

        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; }

        [JsonPropertyName("data_nascimento")]
        public DateTime BirthDate { get; set; }

        [JsonPropertyName("numero_cnh")]
        public string DriverLicenseNumber { get; set; }

        [JsonPropertyName("tipo_cnh")]
        public string DriverLicenseType { get; set; }

        [JsonPropertyName("imagem_cnh")]
        public string DriverLicenseImage { get; set; }
    }
}
