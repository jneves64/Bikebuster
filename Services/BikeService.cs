using BikeBuster.Data;
using BikeBuster.Messaging.Events;
using BikeBuster.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BikeBuster.Services
{
    public class BikeService(DatabaseContext context, IPublishEndpoint publish) : BaseService(context, publish)
    {
        public IEnumerable<BikeModel> GetAll(string? plate)
        {
            var query = this._db.Bike.AsQueryable();
            if (!string.IsNullOrEmpty(plate))
                query = query.Where(m => m.Plate == plate);
            return query.ToList();
        }

        public BikeModel? GetById(string id)
        {
            return _db.Bike.Find(id);
        }


        public async Task<BikeModel> Create(BikeModel moto)
        {
            var entry = await _db.Bike.AddAsync(moto);
            try
            {
                await _db.SaveChangesAsync();
                var saved = entry.Entity; // contém o objeto final com ID do banco
                if (_messageBroker != null){
                    await _messageBroker.Publish(new BikeCreatedEvent(
                        saved.Id,
                        saved.Year,
                        saved.Model,
                        saved.Plate
                    ));
                }
                return saved;

            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505"){
                    throw new InvalidOperationException($"Já existe uma entrada com {pgEx.ConstraintName} ");
                }
                throw;
            }
        }


        public bool UpdatePlate(string id, string newPlate)
        {
            var moto = this._db.Bike.Find(id);
            if (moto == null) return false;
            moto.Plate = newPlate;
            this._db.SaveChanges();
            return true;
        }

        public bool Delete(string id)
        {
            var bike = this._db.Bike.Find(id);
            if (bike == null) return false;
            this._db.Bike.Remove(bike);
            this._db.SaveChanges();
            return true;
        }
    }
}
