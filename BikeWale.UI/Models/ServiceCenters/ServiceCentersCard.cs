using Bikewale.Common;
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
        private uint _makeId, _cityId;
        private readonly IServiceCenter _objSC;

        public ServiceCentersCard(IServiceCenter objSC, uint topCount, uint makeId, uint cityId)
        {
            _objSC = objSC;
            _topCount = topCount;
            _makeId = makeId;
            _cityId = cityId;
        }

        public ServiceCentersCard(IServiceCenter objSC, uint topCount, uint makeId, uint cityId, uint serviceCenterId)
        {
            _objSC = objSC;
            _serviceCenterId = serviceCenterId;
            _topCount = topCount;
            _makeId = makeId;
            _cityId = cityId;
        }

        public ServiceCenterDetailsWidgetVM GetData()
        {
            ServiceCenterDetailsWidgetVM objData = null;
            try
            {
                ServiceCenterData centerData = _objSC.GetServiceCentersByCity(_cityId, (int)_makeId);
                IEnumerable<ServiceCenterDetails> totalList = null;

                if (centerData != null && centerData.ServiceCenters != null)
                {
                    totalList = centerData.ServiceCenters.Where(x => x.ServiceCenterId != _serviceCenterId);

                    if (totalList != null)
                        totalList = totalList.Take((int)_topCount);
                }

                ServiceCenterDetails firstObj = centerData.ServiceCenters.FirstOrDefault();

                objData = new ServiceCenterDetailsWidgetVM();
                objData.ServiceCentersList = totalList;
                if (firstObj != null)
                {
                    objData.MakeMaskingName = firstObj.MakeMaskingName;
                    objData.CityMaskingName = firstObj.CityMaskingName;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ServiceCenters.ServiceCentersCard.GetData");
            }
            return objData;
        }
    }
}