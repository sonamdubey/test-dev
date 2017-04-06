using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.ServiceCenter;
using System;
using System.Collections.Generic;

namespace Bikewale.Models.ServiceCenters
{
    /// <summary>
    /// Created by Sajal Gupta on 24-03-2017
    /// This class provides data for ServiceCentersInNearByCities widget (Dektop + Mobile)
    /// </summary>
    public class ServiceCentersInNearByCities
    {
        private BikeMakeEntityBase _make;
        private uint _topCount, _cityId;
        private readonly IServiceCenterCacheRepository _objSC;

        public ServiceCentersInNearByCities(IServiceCenterCacheRepository objSC, uint topCount, uint cityId, BikeMakeEntityBase make)
        {
            _objSC = objSC;
            _topCount = topCount;
            _cityId = cityId;
            _make = make;
        }

        public ServiceCentersNearByCityWidgetVM GetData()
        {
            ServiceCentersNearByCityWidgetVM objVM = null;

            try
            {
                IEnumerable<CityBrandServiceCenters> ServiceCentersNearbyCities = _objSC.GetServiceCentersNearbyCitiesByBrand((int)_cityId, _make.MakeId, (int)_topCount);

                objVM = new ServiceCentersNearByCityWidgetVM();
                objVM.ServiceCentersNearbyCities = ServiceCentersNearbyCities;
                objVM.Make = _make;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ServiceCenters.ServiceCentersInNearByCities.GetData()");
            }
            return objVM;
        }
    }
}