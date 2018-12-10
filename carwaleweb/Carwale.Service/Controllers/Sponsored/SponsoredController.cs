using Carwale.DTOs.CarData;
using Carwale.Interfaces.SponsoredCar;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using Carwale.Entity;
using Carwale.Entity.Enum;
using Carwale.Utility;
using Carwale.BL.Advertizing;
using System.Web;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using AutoMapper;
using Carwale.Entity.CarData;
using Carwale.DTOs.ES;

namespace Carwale.Service.Controllers.Sponsored
{
    public class SponsoredController : ApiController
    {
        private readonly ISponsoredCarCache _sponsoredCars;
        private readonly SponsoredCampaignApp _sponsoredCampaignApp;

        public SponsoredController(ISponsoredCarCache sponsoredCars, SponsoredCampaignApp sponsoredCampaignApp)
        {
            _sponsoredCars = sponsoredCars;
            _sponsoredCampaignApp = sponsoredCampaignApp;
        }

        [HttpGet, Route("api/sponsored/banners/{screenType}")]
        public IHttpActionResult GetSponsoredBanner(string screenType)
        {
            var sourceId = Request.Headers.Contains("SourceId") ? Request.Headers.GetValues("SourceId").First() : null;
            var appVersion = Request.Headers.Contains("appVersion") ? Request.Headers.GetValues("appVersion").First() : null;

            //android appversion 79 get crashed if this api returns sponsored campaign so we are not returning anything below 79
            if (!Enum.IsDefined(typeof(MobilePlatformScreenType), screenType.ToLower()) || !RegExValidations.IsNumeric(sourceId) || !RegExValidations.IsNumeric(appVersion) || (Convert.ToInt32(sourceId) == (int)Platform.CarwaleAndroid && Convert.ToInt32(appVersion) < 79))
            {
                return BadRequest();
            }

            var campaign = _sponsoredCars.GetSponsoredCampaigns((int)(CampaignCategory.Banner), Convert.ToInt16(sourceId), (int)CategorySection.HomePageBanner);
            if (campaign == null || campaign.Count == 0)
            {
                return BadRequest();
            }

            var sponsoredCarDTO = new SponsoredCarDTO()
            {
                SponsoredAdContentDTO = new List<SponsoredAdContentDTO>() { 
                    new SponsoredAdContentDTO{
                        AdHtmlContent = new List<SponsoredAdHtmlContentDTO>(){
                            new SponsoredAdHtmlContentDTO { HtmlContent = campaign.First().Ad_Html}
                        },
                        AdType = "single",
                        CategoryId = 1 },
                    new SponsoredAdContentDTO{
                        AdHtmlContent = new List<SponsoredAdHtmlContentDTO>(){
                            new SponsoredAdHtmlContentDTO { HtmlContent = campaign.First().Ad_Html}
                        },
                        AdType = "single",
                        CategoryId = 2 }
                }//1-New , 2-Used
            };

            return Ok(sponsoredCarDTO);
        }

        [HttpGet, Route("api/v2/sponsored/banners/{screenType}")]
        public IHttpActionResult GetSponsoredBannerV2(string screenType)
        {
            string param = null, sourceId=null;
            try
            {
                param = HttpContext.Current.Request.QueryString["param"] ?? string.Empty;

                sourceId = Request.Headers.Contains("SourceId") ? Request.Headers.GetValues("SourceId").First() : null;
                if (sourceId == null || !RegExValidations.IsNumeric(sourceId))
                    return BadRequest();

                MobilePlatformScreenType screen = MobilePlatformScreenType.homepage;

                if (!Enum.TryParse<MobilePlatformScreenType>(screenType.ToLower(), out screen))
                    return BadRequest();

                var response = _sponsoredCampaignApp.Response(Convert.ToInt16(sourceId), (int)screen, param);

                if (response == null)
                    return BadRequest();


                return Ok(response);
            }
            catch (Exception ex) 
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "SponsoredController.GetSponsoredBannerV2()screenType:" + screenType ?? "NULL" + ";param:" + param ?? "NULL" + ";sourceId:" + sourceId??"NULL");
                objErr.LogException();
            }
            return InternalServerError();
        }

        /// <summary>
        /// this function fetches sponsored banners and sponsored ads
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="categoryId"></param>
        /// <param name="platformId"></param>
        /// <param name="section"></param>
        /// <param name="param"></param>
        /// <returns>sponsored campaign data</returns>
        [HttpGet, Route("api/v2/sponsored/{applicationId:int:min(1)}")]
        public IHttpActionResult GetSponsoredCampaignAds(int applicationId, int categoryId, int platformId, int sectionId, string param="")
        {            
            try
            {                
                if (platformId < 1 || categoryId < 1)
                    return BadRequest("bad parameters");

                var response = _sponsoredCars.GetSponsoredCampaigns(categoryId, platformId, sectionId, param, applicationId);
                if (response == null || response.Count == 0)
                    return NotFound();
                var data = Mapper.Map<Sponsored_Car, SponsoredAdCampaignDto>(response.First());

                return Ok(data);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);   
                return InternalServerError();
            }            
        }
    }
}
