using BikeBuster.Data;
using BikeBuster.Messaging.Events;
using BikeBuster.Models;
using MassTransit;

namespace BikeBuster.Messaging.Consumers
{
    public class BikeCreatedConsumer : IConsumer<BikeCreatedEvent>
    {
        private readonly DatabaseContext _db;

        public BikeCreatedConsumer(DatabaseContext db) => _db = db;

        public async Task Consume(ConsumeContext<BikeCreatedEvent> context)
        {
            var msg = context.Message;
            Console.WriteLine("consumindo do rabbit");
            if (msg.Ano == 2024)
            {
                await _db.Notification.AddAsync(new NotificationModel
                {
                    BikeId = msg.Identificador
                });
                await _db.SaveChangesAsync();
            }
        }
    }
}