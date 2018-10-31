using Bikewale.DTO.Campaign;
using Bikewale.Interfaces.Campaign;
using Bikewale.Notifications;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Campaign
{
    /// <summary>
    /// Author  : Kartik Rathod on 12 sept 2018
    /// Desc    : to get deatils of campign 
    /// </summary>
    public class CampaignController : ApiController
    {
        private readonly ICampaignBL _campaignBL;

        public CampaignController(ICampaignBL campaignBL)
        {
            _campaignBL = campaignBL;
        }

        /// <summary>
        /// Author  : Kartik Rathod on 12 sept 2018
        /// Desc    : to get deatils of campign location wise
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet, Route("api/campaign/modelId/{modelId}/cityId/{cityId}/"), ResponseType(typeof(ESDSCampaignDto))]
        public IHttpActionResult GetCampaignLocationWise(uint cityId, uint modelId, uint areaId = 0)
        {
            try
            {
                if (cityId > 0 && modelId > 0)
                {
                    ESDSCampaignDto objESDSCampaign = null;
                    objESDSCampaign = _campaignBL.GetCampaignLocationWise(cityId, areaId, modelId);
                    if (objESDSCampaign != null)
                    {
                        return Ok(objESDSCampaign);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest();
                }
                
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.Service.Controllers.Campaign.GetCampaignLocationWise() cityId({0}),areaId({1},modelId({2}))", cityId, areaId, modelId));
                return InternalServerError();
            }
        }
    }
}