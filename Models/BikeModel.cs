using System.ComponentModel.DataAnnotations;

namespace BikeBuster.Models

{
    public class BikeModel
    {
        [Key]
        public string Identificador { get; set; }
        public int Ano { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
    }

}
