using BikeBuster.Data;
using BikeBuster.Messaging.Events;
using BikeBuster.Models;
using MassTransit;

namespace BikeBuster.Messaging.Consumers
{
    public class BikeCreatedConsumer : IConsumer<BikeCreatedEvent>
    {
        private readonly AppDbContext _db;

        public BikeCreatedConsumer(AppDbContext db) => _db = db;

        public async Task Consume(ConsumeContext<BikeCreatedEvent> context)
        {
            var msg = context.Message;
            Console.WriteLine("consumindo do rabbit");
            if (msg.Ano == 2025)
            {
                await _db.Bike.AddAsync(new BikeModel
                {
                    Id = msg.Identificador,
                    Year = msg.Ano,
                    Model = msg.Modelo,
                    Plate = msg.Placa
                });
                await _db.SaveChangesAsync();
            }
        }
    }
}