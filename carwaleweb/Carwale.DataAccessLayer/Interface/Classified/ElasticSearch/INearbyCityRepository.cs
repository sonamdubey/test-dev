using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using System.Collections.Generic;

namespace Carwale.DAL.Interface.Classified.ElasticSearch
{
    public interface INearbyCityRepository
    {
        List<City> GetFromLatLong(double minLat, double maxLat, double minLong, double maxLong, int cityId, ElasticOuptputs filterInputs);
    }
}
