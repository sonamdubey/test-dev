using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.ServiceCenters;
using System;
using System.Collections.Generic;

namespace Bikewale.Models.ServiceCenters
{
    public class ServiceCentersInNearByCities
    {
        private BikeMakeEntityBase _make;
        private uint _topCount, _cityId;

        public ServiceCentersInNearByCities(uint topCount, uint cityId, BikeMakeEntityBase make)
        {
            _topCount = topCount;
            _cityId = cityId;
            _make = make;
        }

        public ServiceCentersNearByCityWidgetVM GetData()
        {
            ServiceCentersNearByCityWidgetVM objVM = null;

            try
            {
                IEnumerable<CityBrandServiceCenters> ServiceCentersNearbyCities = (new BindNearbyCitiesServiceCenters()).GetServiceCentersNearbyCitiesByMake((int)_cityId, _make.MakeId, (int)_topCount);

                objVM = new ServiceCentersNearByCityWidgetVM();
                objVM.ServiceCentersNearbyCities = ServiceCentersNearbyCities;
                objVM.Make = _make;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ServiceCenters.ServiceCentersInNearbyCities.GetData()");
            }
            return objVM;
        }
    }
}