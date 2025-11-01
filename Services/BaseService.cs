using BikeBuster.Data;
using MassTransit;

namespace BikeBuster.Services
{
    public abstract class BaseService(DatabaseContext db, IPublishEndpoint? messageBroker = null)
    {
        protected readonly DatabaseContext _db = db;
        protected readonly IPublishEndpoint? _messageBroker = messageBroker;
    }

}
