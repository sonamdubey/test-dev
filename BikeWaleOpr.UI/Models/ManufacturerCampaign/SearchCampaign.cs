using Bikewale.ManufacturerCampaign.Entities.Models;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models
{
    public class SearchCampaign
    {
        private readonly IManufacturerCampaignRepository _objManufacturer =null;

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
                GetManufacturersList(objVM);
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "SearchCampaign.GetData");
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

                ErrorClass objErr = new ErrorClass(ex, "SearchCampaign.GetManufacturersList");
            }
        }


    }
}