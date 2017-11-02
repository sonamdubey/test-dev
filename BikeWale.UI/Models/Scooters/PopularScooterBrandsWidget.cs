using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.Scooters
{
    /// <summary>
    /// Created by sajal Gupta on 23-08-2017
    /// descr : Model to power popular scooters brands widget
    /// </summary>
    public class PopularScooterBrandsWidget
    {
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        public uint SkipMakeId { get; set; }
        public uint TopCount { get; set; }

        public PopularScooterBrandsWidget(IBikeMakesCacheRepository objMakeCache)
        {
            _objMakeCache = objMakeCache;
        }

        public IEnumerable<BikeMakeEntityBase> GetData()
        {
            IEnumerable<BikeMakeEntityBase> bikeList = null;
            try
            {
                bikeList = _objMakeCache.GetScooterMakes();

                if (bikeList != null && SkipMakeId > 0 && bikeList.Any())
                {
                    bikeList = bikeList.Where(x => x.MakeId != SkipMakeId);
                }

                if (bikeList != null && TopCount > 0 && bikeList.Count() > TopCount)
                {
                    bikeList = bikeList.Take((int)TopCount);
                }

            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "PopularScooterWidget.GetData()");
            }
            return bikeList;
        }
    }
}