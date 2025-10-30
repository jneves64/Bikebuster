using System.ComponentModel.DataAnnotations;

namespace BikeBuster.Models
{
    public class RiderModel
    {
        [Key]
        public string Identificador { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public DateTime Data_Nascimento { get; set; }
        public string Numero_Cnh { get; set; }
        public string Tipo_Cnh { get; set; }
        public string Imagem_Cnh { get; set; }
    }
}
