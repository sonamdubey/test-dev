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
    /// Controller for sponsored camparisons
    /// </summary>
    /// <author>
    /// Sangram Nandkhile on 28-Jul-2017
    /// </author>
    public class SponsoredComparisonController : ApiController
    {
        private readonly ISponsoredCampaignRepository _objSponsoredRepo = null;

        public SponsoredComparisonController(ISponsoredCampaignRepository objSponsoredRepo)
        {
            _objSponsoredRepo = objSponsoredRepo;
        }

        /// <summary>
        /// Gets the campaigns.
        /// </summary>
        /// <param name="statuses">The statuses.</param>
        /// <returns></returns>
        [HttpGet, Route("api/compare/sponsored/{statuses}/")]
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


        [HttpGet, Route("api/compare/sponsored/{id}/target/{targetModelId}/sponsor/{sponsoredModelId}/")]
        public IHttpActionResult GetSponsoredVersionMapping(uint id, uint targetModelId, uint sponsoredModelId)
        {
            TargetSponsoredMapping objSponsoredCampaign = null;
            try
            {
                objSponsoredCampaign = _objSponsoredRepo.GetSponsoredComparisonVersionMapping(id,targetModelId, sponsoredModelId);

                return Ok(objSponsoredCampaign);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Camparison.SponsoredComparisonController.GetSponsoredVersionMapping: Camparison Id: {0}", id));
                return InternalServerError();
            }
        }

        /// <summary>
        /// Saves the sponsored campaign.
        /// </summary>
        /// <param name="campaign">The campaign.</param>
        /// <returns></returns>
        [HttpPost, Route("api/compare/sponsored/save/")]
        public IHttpActionResult SaveSponsoredCampaign([FromBody]SponsoredCampaign campaign)
        {
            bool isSuccess = false;
            try
            {
                isSuccess =  _objSponsoredRepo.SaveSponsoredComparison(campaign);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Camparison.SponsoredComparisonController.SaveSponsoredCampaign");
                return InternalServerError();
            }
            return Ok(isSuccess);
        }

        /// <summary>
        /// Saves the sponsored comparisons bike rules.
        /// </summary>
        /// <param name="ruleObj">The rule object.</param>
        /// <returns></returns>
        [HttpPost, Route("api/compare/sponsored/rules/save/")]
        public IHttpActionResult SaveSponsoredComparisonsBikeRules([FromBody]VersionTargetMapping ruleObj)
        {

            bool isSuccess = false;
            try
            {
                isSuccess = _objSponsoredRepo.SaveSponsoredComparisonBikeRules(ruleObj);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Camparison.SponsoredComparisonController.SaveSponsoredCampaign");
                return InternalServerError();
            }
            return Ok(isSuccess);
        }


        /// <summary>
        /// Deletes the sponsored comparison rules.
        /// </summary>
        /// <param name="comparisonId">The comparison identifier.</param>
        /// <returns></returns>
        [HttpPost, Route("api/compare/sponsored/{comparisonId}/delete/")]
        public IHttpActionResult DeleteSponsoredComparisonRules(uint comparisonId)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objSponsoredRepo.DeleteSponsoredComparisonBikeAllRules(comparisonId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Camparison.SponsoredComparisonController.DeleteSponsoredComparisonRules");
                return InternalServerError();
            }
            return Ok(isSuccess);
        }

        /// <summary>
        /// Deletes the sponsored comparison model rules.
        /// </summary>
        /// <param name="comparisonId">The comparison identifier.</param>
        /// <param name="sponsoredModelId">The sponsored model identifier.</param>
        /// <returns></returns>
        [HttpPost, Route("api/compare/sponsored/{comparisonId}/model/{sponsoredModelId}/rules/delete/")]
        public IHttpActionResult DeleteSponsoredComparisonModelRules(uint comparisonId, uint sponsoredModelId)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objSponsoredRepo.DeleteSponsoredComparisonBikeSponsoredModelRules(comparisonId, sponsoredModelId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Camparison.SponsoredComparisonController.GetCampaigns.DeleteSponsoredComparisonModelRules");
                return InternalServerError();
            }
            return Ok(isSuccess);
        }
    }
}
