using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.ServiceCenters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.ServiceCenters
{
    public class ServiceCentersCard 
    {
        private uint _serviceCenterId, _topCount;
        private CityEntityBase _city;
        private BikeMakeEntityBase _make;

        public ServiceCentersCard(uint topCount, BikeMakeEntityBase make, CityEntityBase city)
        {
            _serviceCenterId = 0;
            _topCount = topCount;
            _make = make;
            _city = city;
        }

        public ServiceCentersCard(uint topCount, uint serviceCenterId, BikeMakeEntityBase make, CityEntityBase city)
        {
            _serviceCenterId = serviceCenterId;
            _topCount = topCount;
            _make = make;
            _city = city;
        }

        public ServiceCenterDetailsWidgetVM GetData()
        {
            ServiceCenterDetailsWidgetVM objData = null;
            try
            {
                BindServiceCenter serviceViewModel = new BindServiceCenter();
                ServiceCenterData centerData = serviceViewModel.GetServiceCenterList(_make.MakeId, _city.CityId);
                IEnumerable<ServiceCenterDetails> totalList = null;

                if (centerData != null && centerData.ServiceCenters != null)
                {
                    totalList = centerData.ServiceCenters.Where(x => x.ServiceCenterId != _serviceCenterId);

                    if (totalList != null)
                        totalList = totalList.Take((int)_topCount);

                }

                objData = new ServiceCenterDetailsWidgetVM();
                objData.ServiceCentersList = totalList;
                objData.MakeMaskingName = _make.MaskingName;
                objData.CityMaskingName = _city.CityMaskingName;

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ServiceCenters.GetData");
            }
            return objData;
        }
    }
}