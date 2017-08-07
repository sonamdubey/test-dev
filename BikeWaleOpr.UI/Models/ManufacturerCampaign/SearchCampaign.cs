using Bikewale.ManufacturerCampaign.Entities.Models;
using Bikewale.ManufacturerCampaign.Interface;
using BikeWaleOpr.Common;
using System;

namespace BikewaleOpr.Models
{
    public class SearchCampaign
    {
        private readonly IManufacturerCampaignRepository _objManufacturer = null;

        public SearchCampaign(IManufacturerCampaignRepository objManufacturer)
        {

            _objManufacturer = objManufacturer;
        }

        /// <summary>
        /// Modified by :- Subodh Jain 10 july 2017
        /// summary :- Get manufacturer list
        /// </summary>
        /// <returns></returns>
        public SearchManufacturerCampaignVM GetData()
        {
            SearchManufacturerCampaignVM objVM = new SearchManufacturerCampaignVM();
            try
            {
                objVM.UserId = CurrentUser.Id;
                GetManufacturersList(objVM);
            }
            catch (Exception ex)
            {

                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(ex, "SearchCampaign.GetData");
            }
            return objVM;

        }
        /// <summary>
        /// Modified by :- Subodh Jain 10 july 2017
        /// summary :- Get manufacturer list
        /// </summary>
        /// <returns></returns>
        private void GetManufacturersList(SearchManufacturerCampaignVM objVM)
        {
            try
            {
                objVM.ManufacturerList = _objManufacturer.GetManufacturersList();
            }
            catch (Exception ex)
            {

                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(ex, "SearchCampaign.GetManufacturersList");
            }
        }


    }
}