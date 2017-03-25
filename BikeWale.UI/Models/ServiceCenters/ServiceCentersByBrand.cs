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
    /// This class provides data for ServiceCentersByBrand widget (Desktop + Mobile)
    /// </summary>
    public class ServiceCentersByBrand
    {
        private uint _makeId;
        private readonly IServiceCenterCacheRepository _serviceCenterObj;

        public ServiceCentersByBrand(IServiceCenterCacheRepository serviceCenterObj, uint makeId)
        {
            _serviceCenterObj = serviceCenterObj;
            _makeId = makeId;
        }

        public IEnumerable<BrandServiceCenters> GetData()
        {
            IEnumerable<BrandServiceCenters> AllServiceCenters = null;
            try
            {
                AllServiceCenters = _serviceCenterObj.GetAllServiceCentersByBrand();

                if (AllServiceCenters != null && AllServiceCenters.Count() > 0)
                    AllServiceCenters = AllServiceCenters.Where(v => v.MakeId != _makeId);

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ServiceCenters.ServiceCentersByBrand.GetData()");
            }
            return AllServiceCenters;
        }
    }
}