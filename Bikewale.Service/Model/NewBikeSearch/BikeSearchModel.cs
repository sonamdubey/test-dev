
using Bikewale.ElasticSearch.Entities;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Service.Model.NewBikeSearch
{
    public class BikeSearchModel
    {
        public IEnumerable<BikeModelDocument> GetData(IEnumerable<BikeModelDocument> bikeList, IEnumerable<BikeModelDocument> bikeListWithCityPrice)
        {

            for (int index = 0; index < bikeList.Count(); index++)
            {
                bikeList.ElementAt(index).TopVersion = bikeListWithCityPrice.ElementAt(index).TopVersion;
            }

            return bikeList;
        }
    }
}