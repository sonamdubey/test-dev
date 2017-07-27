using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bikewale.Comparison.Entities;
using Bikewale.Notifications;
using Bikewale.Comparison.Interface;

namespace BikewaleOpr.Service.Controllers.Camparison
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 26 July 2017
    /// Summary:
    /// </summary>
    public class SponsoredComparisonController : ApiController
    {
        private readonly ISponsoredCampaignRepository _objSponsoredRepo = null;

        public SponsoredComparisonController(ISponsoredCampaignRepository objSponsoredRepo)
        {
            _objSponsoredRepo = objSponsoredRepo;
        }
        /// <summary>
        /// Created by: Sangram Nandkhile on 26 July 2017
        /// Summary: Controller for sponsored comparison
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpGet, Route("api/comparison/sponsoredcampaigns/{statuses}/")]
        public IHttpActionResult GetCampaigns(string statuses)
        {
           IEnumerable<SponsoredCampaign> objSponsoredCampaign = null;
            try
            {
                objSponsoredCampaign = _objSponsoredRepo.GetSponsoredComparisons(statuses);

                return Ok(objSponsoredCampaign);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Camparison.SponsoredComparisonController.GetCampaigns", statuses));
                return InternalServerError();
            }
        }

        [HttpPost, Route("api/comparison/sponsoredcampaigns/save/")]
        public IHttpActionResult SaveSponsoredCampaign([FromBody]SponsoredCampaign campaign)
        {
            bool isSuccess = false;
            try
            {
                isSuccess =  _objSponsoredRepo.SaveSponsoredComparison(campaign);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Camparison.SponsoredComparisonController.GetCampaigns.SaveSponsoredCampaign");
                return InternalServerError();
            }
            return Ok(isSuccess);
        }


    }
}
