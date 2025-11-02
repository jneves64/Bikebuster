using System.Text.RegularExpressions;
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
        public async Task<IEnumerable<BikeModel>> GetAllAsync(string? plate)
        {
            if (!string.IsNullOrWhiteSpace(plate))
                plate = Regex.Replace(plate.ToUpper(), "[^A-Z0-9]", "");

            var query = _db.Bike.AsQueryable();
            if (!string.IsNullOrEmpty(plate))
                query = query.Where(m => m.Plate == plate);
            return await query.ToListAsync();
        }

        public async Task<BikeModel?> GetByIdAsync(string id)
        {
            return await _db.Bike.FindAsync([id]);
        }


        public async Task<BikeModel> Create(BikeModel moto)
        {
            var entry = await _db.Bike.AddAsync(moto);

            await _db.SaveChangesAsync();
            var saved = entry.Entity;
            if (_messageBroker != null)
            {
                await _messageBroker.Publish(new BikeCreatedEvent(
                    saved.Id,
                    saved.Year,
                    saved.Model,
                    saved.Plate
                ));
            }
            return saved;
        }


        public async Task<bool> UpdatePlateAsync(string id, string plate)
        {
            if (!string.IsNullOrWhiteSpace(plate))
                plate = Regex.Replace(plate.ToUpper(), "[^A-Z0-9]", "");
            if (plate.Count() > 7 || plate.Count() < 7)
                throw new InvalidOperationException("placa inválida, precisa ter 7 digitos");

            var moto = await _db.Bike.FindAsync(id);
            if (moto == null) return false;
            moto.Plate = plate;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var bike = await _db.Bike.FindAsync(id);
            if (bike == null) return false;
            _db.Bike.Remove(bike);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}