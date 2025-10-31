using BikeBuster.Data;
using BikeBuster.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeBuster.Services
{
    public class BikeService
    {
        private readonly AppDbContext _context;
        public BikeService(AppDbContext context) => _context = context;

        public IEnumerable<BikeModel> GetAll(string? plate)
        {
            var query = _context.Bike.AsQueryable();
            if (!string.IsNullOrEmpty(plate))
                query = query.Where(m => m.Plate == plate);
            return query.ToList();
        }

        public BikeModel? GetById(string id) => _context.Bike.Find(id);

        public BikeModel Create(BikeModel moto)
        {
            _context.Bike.Add(moto);
            _context.SaveChanges();
            return moto;
        }

        public bool UpdatePlate(string id, string newPlate)
        {
            var moto = _context.Bike.Find(id);
            if (moto == null) return false;
            moto.Plate = newPlate;
            _context.SaveChanges();
            return true;
        }

        public bool Delete(string id)
        {
            var moto = _context.Bike.Find(id);
            if (moto == null) return false;
            _context.Bike.Remove(moto);
            _context.SaveChanges();
            return true;
        }
    }
}
