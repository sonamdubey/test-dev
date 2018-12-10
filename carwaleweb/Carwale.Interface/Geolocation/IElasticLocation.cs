using Carwale.Entity.Geolocation;

namespace Carwale.Interfaces.Geolocation
{
    public interface IElasticLocation
    {
        Area GetLocation(double latitude, double longitude);
        Area GetLocation(int areaId);
        Location FormCompleteLocation(Location locationObj);
    }
}
