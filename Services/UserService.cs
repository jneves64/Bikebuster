using BikeBuster.Data;
using BikeBuster.Messaging.Events;
using BikeBuster.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace BikeBuster.Services
{
    public class UserService(DatabaseContext context) : BaseService(context)
    {

        public async Task<UserModel> Create(UserModel user)
        {
            var entry = await _db.User.AddAsync(user);

            await _db.SaveChangesAsync();
            var saved = entry.Entity;

            return saved;
        }

        public async Task<UserModel?> GetByIdAsync(string id)
        {
            return await _db.User.FindAsync([id]);
        }

        public async Task<bool> UpdateLicenseImageAsync(string userId, string base64Image, CancellationToken cancellationToken = default)
        {
            var base64Sanitized = base64Image.Contains(",") ? base64Image.Split(',')[1] : base64Image;

            // Valida Base64
            byte[] bytes;

            try
            {
                bytes = Convert.FromBase64String(base64Sanitized);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Imagem inválida (Base64 incorreto)");
            }

            // Busca usuário
            var user = await _db.User.FindAsync(new object[] { userId }, cancellationToken);
            if (user == null)
                return false;

            // Salva arquivo
            var folder = Path.Combine("Storage", "CNH");
            Directory.CreateDirectory(folder);
            var filePath = Path.Combine(folder, $"{userId}.png");
            await File.WriteAllBytesAsync(filePath, bytes, cancellationToken);

            // Atualiza banco
            user.DriverLicenseImage = filePath;
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }

    }

}
