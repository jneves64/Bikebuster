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
                rent.Id = RandomIDGenerator(targetUser.Cnpj, targetBike.Plate);

            rent.ContractEndDate = null;
            rent.DailyRate = GetDailyRate(rent.Plan);
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
                return (expectedDays * dailyRate) + (extraDays * 50m);
            }
            else
            {
                return expectedDays * dailyRate;
            }
        }
        private static decimal GetDailyRate(RentalPlan plan) => plan switch
        {
            RentalPlan.SevenDays => 30m,
            RentalPlan.FifteenDays => 28m,
            RentalPlan.ThirtyDays => 22m,
            RentalPlan.FortyFiveDays => 20m,
            RentalPlan.FiftyDays => 18m,
            _ => throw new InvalidOperationException("Plano inválido")
        };

        private static decimal GetPenaltyPercent(RentalPlan plan) => plan switch
        {
            RentalPlan.SevenDays => 0.20m,
            RentalPlan.FifteenDays => 0.40m,
            RentalPlan.ThirtyDays => 0m,
            RentalPlan.FortyFiveDays => 0m,
            RentalPlan.FiftyDays => 0m,
            _ => throw new InvalidOperationException("Plano inválido")
        };


        private static string RandomIDGenerator(string cnpj, string bikePlate)
        {
            string dados = $"{cnpj}{bikePlate}{DateTime.UtcNow.Ticks}";
            int hash = dados.GetHashCode();

            string guidPart = Guid.NewGuid().ToString("N").Substring(0, 2);

            return $"{Math.Abs(hash):X4}{guidPart}".ToUpper();
        }
    }
}
