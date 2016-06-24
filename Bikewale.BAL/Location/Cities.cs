using Bikewale.DAL.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Bikewale.BAL.Location
{
    public class Cities
    {
        private readonly ICity objCities = null;

        public Cities()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICity, CityRepository>();
                objCities = container.Resolve<ICity>();
            }
        }

        public List<CityEntityBase> GetPriceQuoteCities(uint modelId) { throw new NotImplementedException(); }
        public List<CityEntityBase> GetAllCities(EnumBikeType requestType) { throw new NotImplementedException(); }
        public List<CityEntityBase> GetCities(string stateId, EnumBikeType requestType) { throw new NotImplementedException(); }
        public Hashtable GetMaskingNames() { throw new NotImplementedException(); }
        public Hashtable GetOldMaskingNames() { throw new NotImplementedException(); }

        public DealerStateCities GetDealerStates(uint makeId, uint stateId)
        {
            return objCities.GetDealerStateCities(makeId, stateId);
        }
    }
}
