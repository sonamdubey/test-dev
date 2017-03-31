using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Interfaces.ServiceCenter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.ServiceCenters
{
    /// <summary>
    /// Created by Sajal Gupta on 24-03-2017
    /// This class provides data for ServiceCentersCard widget (Desktop + Mobile)
    /// </summary>
    public class ServiceCentersCard
    {
        private uint _serviceCenterId, _topCount;
        private CityEntityBase _city;
        private BikeMakeEntityBase _make;
        private readonly IServiceCenter _objSC;


        public ServiceCentersCard(IServiceCenter objSC, uint topCount, BikeMakeEntityBase make, CityEntityBase city)
        {
            _objSC = objSC;
            _topCount = topCount;
            _make = make;
            _city = city;
        }

        public ServiceCentersCard(IServiceCenter objSC, uint topCount, uint serviceCenterId, BikeMakeEntityBase make, CityEntityBase city)
        {
            _objSC = objSC;
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
                ServiceCenterData centerData = _objSC.GetServiceCentersByCity(_city.CityId, _make.MakeId);
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
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ServiceCenters.ServiceCentersCard.GetData");
            }
            return objData;
        }
    }
}