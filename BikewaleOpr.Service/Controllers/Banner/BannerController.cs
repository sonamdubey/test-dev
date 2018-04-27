using Bikewale.Notifications;
using BikewaleOpr.BAL;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface.Banner;
using BikewaleOpr.Models;
using System;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers
{
    /// <summary>
    /// Created By :- Subodh Jain on 24 july 2017
    /// Summary :- Banner Comtroller
    /// </summary>
    public class BannerController : ApiController
    {
        private readonly IBannerRepository _objBannerRespository = null;

        public BannerController(IBannerRepository objBannerRespository)
        {
            _objBannerRespository = objBannerRespository;
        }

        /// <summary>
        /// Created By :- Subodh Jain on 24 july 2017
        /// Summary :- Banner SaveBannerBasicDetails
        /// Modified By : Rajan Chauhan on 26 Apr 2018
        /// Description : Added Cache key removal logic on save
        /// </summary>
        [HttpPost, Route("api/bannerbasic/save/")]
        public IHttpActionResult SaveBannerBasicDetails([FromBody] BannerVM objBanner)
        {
            uint campaignid = 0;
            try
            {
                campaignid = _objBannerRespository.SaveBannerBasicDetails(objBanner);
                MemCachedUtil.Remove("BW_HomePageBanner_V1_PlatformId_0");
                MemCachedUtil.Remove("BW_HomePageBanner_V1_PlatformId_1");
                return Ok(campaignid);
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "BikewaleOpr.Service.Controllers.SaveBannerBasicDetails");

                return InternalServerError();
            }
        }

        /// <summary>
        /// Modified By : Rajan Chauhan on 26 Apr 2018
        /// Description : Changed cache key version from BW_HomePageBanner_PlatformId_0 
        ///               to BW_HomePageBanner_V1_PlatformId_0
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="bannerStatus"></param>
        /// <returns></returns>
        [HttpPost, Route("api/banner/changeStatus/{reviewId}/{bannerStatus}")]
        public IHttpActionResult ChangeBannerStatus(uint reviewId, UInt16 bannerStatus)
        {
            bool status = false;
            try
            {
                status = _objBannerRespository.ChangeBannerStatus(reviewId, bannerStatus);
                MemCachedUtil.Remove("BW_HomePageBanner_V1_PlatformId_0");
                MemCachedUtil.Remove("BW_HomePageBanner_V1_PlatformId_1");
                return Ok(status);
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "BikewaleOpr.Service.Controllers.StopBanner");
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created By :- Subodh Jain on 24 july 2017
        /// Summary :- Banner SaveBanner properties (desktop and mobile)
        /// </summary>
        [HttpPost, Route("api/bannerproperties/save/{platformId}/")]
        public IHttpActionResult SaveBanner([FromBody] BannerVM objBanner, uint platformId)
        {
            if (platformId > 0)
            {
                try
                {
                    BannerDetails objBannerDetails = (platformId == 1) ? objBanner.DesktopBannerDetails : objBanner.MobileBannerDetails;
                    bool success = _objBannerRespository.SaveBannerProperties(objBannerDetails, platformId, objBanner.CampaignId);
                    if (success)
                    {
                        MemCachedUtil.Remove(string.Format("BW_HomePageBanner_V1_PlatformId_{0}", platformId));
                    }

                    return Ok(success);
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "BikewaleOpr.Service.Controllers.SaveBanner");

                    return InternalServerError();
                }
            }
            else
            {
                return BadRequest("Invalid inputs");
            }
        }
    }
}
