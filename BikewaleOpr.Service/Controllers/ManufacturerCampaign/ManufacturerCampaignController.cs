using Bikewale.Notifications;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Interface.ManufacturerCampaign;
using System;
using System.Collections.Generic;
using System.Web.Http;
namespace BikewaleOpr.Service.Controllers
{  /// <summary>
    /// Created by Subodh Jain 29 aug 2016
    /// Description :For Manufacturer Campaign
    /// </summary>
    /// <param name="dealerId"></param>
    /// <returns></returns>
    public class ManufacturerCampaignController : ApiController
    {
        private readonly IManufacturerCampaign _objManufacturerCampaign = null;
        public ManufacturerCampaignController(IManufacturerCampaign objManufacturerCampaign)
        {
            _objManufacturerCampaign = objManufacturerCampaign;
        }
        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : return list of campaigns for selected dealer
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetManufactureCampaigns(uint dealerId)
        {
            IEnumerable<ManufactureDealerCampaign> _objMfgList = null;
            try
            {
                if (dealerId > 0)
                {
                    _objMfgList = _objManufacturerCampaign.SearchManufactureCampaigns(dealerId);
                    return Ok(_objMfgList);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "SearchManufactureCampaign");
                objErr.SendMail();
                return InternalServerError();
            }            
        }
        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : Change status for Campaign
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetstatuschangeCampaigns(uint id, uint isactive)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objManufacturerCampaign.statuschangeCampaigns(id, isactive);
            }
            catch (Exception ex)
            {
              ErrorClass objErr = new ErrorClass(ex, "statuschangeCampaigns");
                objErr.SendMail();
                return InternalServerError();
            }
            return Ok(isSuccess);
        }


    }
}
