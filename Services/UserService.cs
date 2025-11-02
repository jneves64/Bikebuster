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
            string imgPath = "";
            if (!string.IsNullOrEmpty(user.DriverLicenseImage))
            {
                imgPath = await this.SaveLicenseImageFileAsync(user.Id, user.DriverLicenseImage);
            }
            user.DriverLicenseImage = imgPath;
            var entry = await _db.User.AddAsync(user);

            await _db.SaveChangesAsync();
            var saved = entry.Entity;
            return saved;
        }

        public async Task<UserModel?> GetByIdAsync(string id)
        {
            return await _db.User.FindAsync([id]);
        }

        public async Task<bool> UpdateLicenseImageAsync(string userId, string base64Image)
        {
            var user = await _db.User.FindAsync([userId]);
            if (user == null) return false;

            user.DriverLicenseImage = await SaveLicenseImageFileAsync(
                userId,
                base64Image
            );
            await _db.SaveChangesAsync();
            return true;
        }

        private async Task<string> SaveLicenseImageFileAsync(string userId, string base64Image)
        {
            var base64Sanitized = base64Image.Contains(",")
                ? base64Image.Split(',')[1]
                : base64Image;

            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(base64Sanitized);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Imagem inválida (Base64 incorreto)");
            }

            var folder = Path.Combine("InternalStorage", "CNH");
            Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, $"{userId}.png");
            await File.WriteAllBytesAsync(filePath, bytes);
            return filePath;
        }

    }

}
