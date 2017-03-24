using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.Entities.ServiceCenters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.ServiceCenters 
{
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
                BindOtherBrandsServiceCenters servicecentViewModel = new BindOtherBrandsServiceCenters();
                AllServiceCenters = servicecentViewModel.GetAllServiceCentersbyMake();

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