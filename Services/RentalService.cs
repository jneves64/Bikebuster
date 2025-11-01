using BikeBuster.Data;
using BikeBuster.Messaging.Events;
using BikeBuster.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace BikeBuster.Services
{
    public class RentalService(DatabaseContext context) : BaseService(context)
    {
    }
}
