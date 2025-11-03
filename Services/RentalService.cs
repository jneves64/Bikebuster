using System.ComponentModel.DataAnnotations;
using BikeBuster.Data;
using BikeBuster.Messaging.Events;
using BikeBuster.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace BikeBuster.Services
{
    public class RentalService(DatabaseContext context, UserService userService, BikeService bikeService) : BaseService(context)
    {
        private readonly UserService _userService = userService;
        private readonly BikeService _bikeService = bikeService;

        public async Task<RentalModel> Create(RentalModel rent)
        {
            var targetUser = await _userService.GetByIdAsync(rent.UserId);
            var targetBike = await _bikeService.GetByIdAsync(rent.BikeId);
            var alreadyRented = await this.IsBikeRentedAsync(rent.BikeId);


            if (targetUser == null)
                throw new InvalidOperationException("Usuário não encontrado");
            if (targetBike == null)
                throw new InvalidOperationException("Moto não encontrada");
            if (alreadyRented)
                throw new InvalidOperationException("Moto já está alugada");

            if (targetUser.DriverLicenseType != DriverLicenseType.A && targetUser.DriverLicenseType != DriverLicenseType.AB)
                throw new InvalidOperationException("Usuário não tem Habilitação tipo A");

            if (rent.Id == null)
                rent.Id = this.RandomIDGenerator(targetUser.Cnpj, targetBike.Plate);
            
            rent.ContractEndDate = null;
            var entry = await _db.Rent.AddAsync(rent);
            await _db.SaveChangesAsync();
            var saved = entry.Entity;

            return saved;
        }

        public Task<bool> IsBikeRentedAsync(string bikeId)
        {
            //var now = DateTime.UtcNow;
            return _db.Rent
                .AnyAsync(r =>
                    r.BikeId == bikeId &&
                    r.ContractEndDate == null
                    );
        }

        public async Task<RentalModel?> GetByIdAsync(string id)
        {
            return await _db.Rent.FindAsync([id]);
        }

        public async Task<(RentalModel rental, decimal totalValue)> ReturnAsync(string rentalId, DateTime newReturnDate)
        {
            var rental = await _db.Rent.FindAsync(rentalId) ?? throw new KeyNotFoundException("Locação não encontrada");
            var total = CalculateTotal(rental, newReturnDate);
            rental.ContractEndDate = newReturnDate;
            await _db.SaveChangesAsync();

            return (rental, total);
        }

        public decimal CalculateTotal(RentalModel rental, DateTime returnDate)
        {
            var daysUsed = (returnDate - rental.ContractStartDate).Days;
            var expectedDays = (rental.ContractExpectedEndDate - rental.ContractStartDate).Days;

            if (daysUsed < expectedDays)
            {
                var (_, penaltyPercent) = GetPlan(expectedDays);
                var unusedDays = expectedDays - daysUsed;
                var unusedValue = unusedDays * rental.DailyRate;
                var penalty = unusedValue * penaltyPercent;
                return (daysUsed * rental.DailyRate) + penalty;
            }
            else if (daysUsed > expectedDays)
            {
                var extraDays = daysUsed - expectedDays;
                return (expectedDays * rental.DailyRate) + (extraDays * 50);
            }
            else
            {
                return expectedDays * rental.DailyRate;
            }
        }
        private (decimal dailyRate, decimal penaltyPercent) GetPlan(int days) => days switch
        {
            7 => (30m, 0.20m),
            15 => (28m, 0.40m),
            30 => (22m, 0m),
            45 => (20m, 0m),
            50 => (18m, 0m),
            _ => throw new ArgumentException("Plano inválido")
        };


        private string RandomIDGenerator(string cnpj, string bikePlate)
        {
            string dados = $"{cnpj}{bikePlate}{DateTime.UtcNow.Ticks}";
            int hash = dados.GetHashCode();

            string guidPart = Guid.NewGuid().ToString("N").Substring(0, 2);

            return $"{Math.Abs(hash):X4}{guidPart}".ToUpper();
        }
    }
}
