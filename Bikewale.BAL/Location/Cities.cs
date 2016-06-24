using Bikewale.Cache.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.BAL.Location
{
    public class Cities : ICity
    {
        private readonly ICityCacheRepository objCities = null;

        public Cities()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICityCacheRepository, CityCacheRepository>();
                objCities = container.Resolve<ICityCacheRepository>();
            }
        }

        public List<CityEntityBase> GetPriceQuoteCities(uint modelId) { throw new NotImplementedException(); }
        public List<CityEntityBase> GetAllCities(EnumBikeType requestType) { throw new NotImplementedException(); }
        public List<CityEntityBase> GetCities(string stateId, EnumBikeType requestType) { throw new NotImplementedException(); }
        public Hashtable GetMaskingNames() { throw new NotImplementedException(); }
        public Hashtable GetOldMaskingNames() { throw new NotImplementedException(); }

        /// <summary>
        /// Created by : Vivek Gupta
        /// Date : 24 june 2016
        /// Desc : get dealer cities for dealer locatr
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public DealerStateCities GetDealerStateCities(uint makeId, uint stateId)
        {
            return objCities.GetDealerStateCities(makeId, stateId);
        }
    }
}
