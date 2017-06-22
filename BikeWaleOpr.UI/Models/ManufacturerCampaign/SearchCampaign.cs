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
        private readonly IManufacturerCampaign _objManufacturer=null;

        public SearchCampaign(IManufacturerCampaign objManufacturer)
        {

            _objManufacturer = objManufacturer;
        }


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