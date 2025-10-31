using BikeBuster.Models;

namespace BikeBuster.Messaging.Events
{

    public record BikeCreatedEvent(string Identificador, int Ano, string Modelo, string Placa)
    {
        public static BikeCreatedEvent FromModel(BikeModel bike) =>
            new(bike.Id, bike.Year, bike.Model, bike.Plate);
    }
}