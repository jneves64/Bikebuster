using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;


namespace BikeBuster.Models
{
    public class NotificationModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string BikeId { get; set; }

        [Required]
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }
}
