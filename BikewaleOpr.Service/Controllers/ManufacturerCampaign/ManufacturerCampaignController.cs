using Bikewale.Notifications;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Interface.ManufacturerCampaign;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers.ManufacturerCamapaigns
{  
    /// <summary>
    /// Created by Subodh Jain 29 aug 2016
    /// Description :For Manufacturer Campaign
    /// </summary>    
    public class ManufacturerCampaignController : ApiController
    {
        private readonly IManufacturerCampaignRepository _objManufacturerCampaign = null;
        
        public ManufacturerCampaignController(IManufacturerCampaignRepository objManufacturerCampaign)
        {
            _objManufacturerCampaign = objManufacturerCampaign;
        }

        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : return list of campaigns for selected dealer
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpGet, Route("api/campaigns/manufacturer/search/dealerId/{dealerId}")]
        public IHttpActionResult GetCampaigns(uint dealerId)
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
                ErrorClass objErr = new ErrorClass(ex, "ManufacturerCampaignController.GetCampaigns");
                objErr.SendMail();
                return InternalServerError();
            }
        }
        /// <summary>
        /// Created by Subodh Jain 29 aug 2016
        /// Description : API to make manufacturer campaign active or inactive
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpPost, Route("api/campaigns/manufacturer/updatecampaignstatus/campaignId/{campaignId}/status/{isactive}")]
        public IHttpActionResult UpdateCampaignStatus(uint campaignId, bool isactive)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objManufacturerCampaign.UpdateCampaignStatus(campaignId, isactive);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ManufacturerCampaignController.UpdateCampaignStatus");
                objErr.SendMail();
                return InternalServerError();
            }
            return Ok(isSuccess);
        }


    }
}
