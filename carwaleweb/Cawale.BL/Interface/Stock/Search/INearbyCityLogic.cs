using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using System.Collections.Generic;

namespace Carwale.BL.Interface.Stock.Search
{
    public interface INearbyCityLogic
    {
        List<City> GetCities(ElasticOuptputs filterInputs);
        string GetNearbyCityText(int cityId, int cityCount);
    }
}
