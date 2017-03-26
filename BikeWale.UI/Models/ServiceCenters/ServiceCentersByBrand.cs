using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.Entities.ServiceCenters;
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

        public ServiceCentersByBrand(uint makeId)
        {
            _makeId = makeId;
        }

        public IEnumerable<BrandServiceCenters> GetData()
        {
            IEnumerable<BrandServiceCenters> AllServiceCenters = null;
            try
            {
                AllServiceCenters = (new BindOtherBrandsServiceCenters()).GetAllServiceCentersbyMake();

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