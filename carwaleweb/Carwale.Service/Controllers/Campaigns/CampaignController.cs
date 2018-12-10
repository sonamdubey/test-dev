using AutoMapper;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.Dealer;
using Carwale.Entity.Campaigns;
using Carwale.Entity.Dealers;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.Dealers;
using Carwale.Notifications;
using Carwale.Service.Filters;
using Carwale.Service.Filters.Campaigns;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Carwale.Entity.Geolocation;
using Carwale.Notifications.Logs;
using Carwale.BL.Campaigns;

namespace Carwale.Service.Controllers.Campaigns
{
    public class CampaignController : ApiController
    {
        private readonly ICampaign _campaign;
        private readonly ICampaignRecommendationsBL _campaignRecommendationsBl;
        private readonly INewCarDealers _newCarDealers;
        private readonly ITemplate _campaignTemplate;

        public CampaignController(ICampaign campaign, ICampaignRecommendationsBL campaignRecommendationsBl, INewCarDealers newCarDealers, ITemplate campainTemplate)
        {
            _campaign = campaign;
            _campaignRecommendationsBl = campaignRecommendationsBl;
            _newCarDealers = newCarDealers;
            _campaignTemplate = campainTemplate;
        }

        [Route("api/campaign/{Id}")]
        public IHttpActionResult Get(int id)
        {
            var sourceId = Request.Headers.Contains("SourceId") ? Request.Headers.GetValues("SourceId").First() : "-1";

            if (!RegExValidations.IsPositiveNumber(sourceId))
            {
                return BadRequest();
            }

            var campaign = _campaign.GetCampaignDetails(id);

            var campaignDto = Mapper.Map<Campaign, CampaignDTO>(campaign);

            return Ok(campaignDto);
        }

        [HttpGet, Route("api/campaign/random")]
        public IHttpActionResult GetRandomCampaign(int modelId, int cityId, int platformId, int zoneId = 0)
        {
            var locationObj = new Location { CityId = cityId, ZoneId = zoneId };
            var campaign = _campaign.GetCampaignByCarLocation(modelId, locationObj, platformId, false);

            if (!CampaignValidation.IsCampaignValid(campaign))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            campaign = _campaign.GetCampaignWithScore(campaign, HttpContext.Current.Request, modelId, cityId, zoneId, platformId);
            var campaignDto = Mapper.Map<Campaign, CampaignDTO>(campaign);

            return Ok(campaignDto);
        }

        /// <summary>
        /// For getting Campaign recommendations on submitting a lead
        /// Created: Vicky Lund, 01/12/2015
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/campaign/recommendations")]
        public IHttpActionResult GetCampaignRecommendations(string models, string mobile, int recommendationcount)
        {
            if (mobile == "" || recommendationcount < 1 || !RegExValidations.IsValidMobile(mobile))
            {
                return BadRequest();
            }

            var campaignRecommendations = _campaignRecommendationsBl.GetCampaignRecommendationsByLead(models, mobile, recommendationcount);
            return Ok(campaignRecommendations);
        }

        [HttpGet, Route("api/v1/campaign/recommendations")]
        public IHttpActionResult GetCampaignRecommendations(int modelId, short platformId, int recommendationcount, int cityId, string mobile = "", int? areaId = 0, int zoneId = 0, bool boost = false, bool isCheckRecommendation = true, bool isSameMakeFilter = true)
        {
            try
            {
                string cwcCookie = HttpContext.Current.Request.Headers["IMEI"] ?? UserTracker.GetSessionCookie();
                if (recommendationcount < 1 || platformId <= 0 || cityId <= 0 || modelId <= 0)
                {
                    return BadRequest();
                }

                CampaignInput campaignInput = new CampaignInput
                {
                    CityId = cityId,
                    AreaId = CustomParser.parseIntObject(areaId) > 0 ? CustomParser.parseIntObject(areaId) : 0,
                    ModelId = modelId,
                    PlatformId = platformId,
                    ZoneId = zoneId
                };
                var campaignRecommendations = _campaignRecommendationsBl.GetCampaignRecommendation(mobile, recommendationcount, campaignInput, cwcCookie, boost, isCheckRecommendation, isSameMakeFilter);
                var campaignRecommendationsDto = Mapper.Map<List<CampaignRecommendation>, List<CampaignRecommendationDTO>>(campaignRecommendations);

                if (campaignRecommendationsDto == null || campaignRecommendationsDto.Count <= 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }

                return Ok(campaignRecommendationsDto);
            }
            catch (Exception err)
            {
                Logger.LogException(err, HttpContext.Current.Request.ServerVariables["URL"]);
                return InternalServerError();
            }
        }
        /// <summary>
        /// For getting Similar Cars on basis of bodystyle and subsegment of ModelId passed 
        /// Created: Sanjay Soni, 22/03/2016
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/campaign/similarcar")]
        public IHttpActionResult GetSimilarRecommendCar(int modelId, int cityId, bool isSamebodyStyle, int zoneId = -1, int subsegmentRange = -1, int recommendationCount = -1)
        {
            if (!RegExValidations.IsPositiveNumber(CustomParser.parseStringObject(modelId))
                || !RegExValidations.IsPositiveNumber(CustomParser.parseStringObject(cityId))
                || zoneId < -1 || zoneId == 0 || subsegmentRange < -1 || recommendationCount < -1)
            {
                return BadRequest();
            }
            var locationObj = new Location { CityId = cityId, ZoneId = zoneId };
            var Similarcampaign = _campaignRecommendationsBl.SimilarCampaignRecommend(modelId, locationObj, subsegmentRange, recommendationCount, isSamebodyStyle);

            return Ok(Similarcampaign);
        }

        [HttpGet, CampaignFilter, Route("api/campaigns")]
        public IHttpActionResult GetCampaigns(int modelId, int cityId, int zoneId, int sourceId)
        {
            IList<Campaign> campaign = _campaign.GetAllCampaign(modelId, new Location { CityId = cityId, ZoneId = zoneId }, sourceId);
            var campaignDto = Mapper.Map<IEnumerable<Campaign>, IEnumerable<CampaignDTO>>(campaign);
            return Ok(campaignDto);
        }

        [EnableCors(origins: "http://localhost,https://localhost,http://test.cartrade.com,https://test.cartrade.com,http://testm.cartrade.com,"
            + "https://testm.cartrade.com,http://testapi.cartrade.com,https://testapi.cartrade.com,http://www.cartrade.com,https://www.cartrade.com,"
            + "http://m.cartrade.com,https://m.cartrade.com,http://api.cartrade.com,https://api.cartrade.com", headers: "*", methods: "GET")]
        [HttpGet, Route("api/v2/campaigns")]
        public IHttpActionResult GetCampaignsV2(int modelId = -1, int cityId = 0, int platformId = -1, int applicationId = -1, int? zoneId = 0, int count = 0)
        {
            try
            {
                if (modelId < 1 || cityId < -1 || zoneId < 0 || count < 0 || platformId < 1 || applicationId < 1)
                {
                    return BadRequest();
                }

                Location locationObj = new Location { CityId = cityId, ZoneId = CustomParser.parseIntObject(zoneId) };

                IEnumerable<CampaignDTOv2> campaign = _campaign.GetAllCampaignV2(modelId, locationObj, platformId, applicationId, count);

                if (campaign == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }

                return Ok(campaign);
            }
            catch (Exception e)
            {
                var exception = new ExceptionHandler(e, HttpContext.Current.Request.ServerVariables["URL"]);
                exception.LogException();
                return InternalServerError();
            }
        }

        [HttpGet, Route("api/campaigns/exists")]
        public IHttpActionResult GetCampaignsAvailbility(int modelId, int cityId, int zoneId = 0, int sourceId = 1, int applicationId = 3)
        {
            Location locationObj = new Location { CityId = cityId, ZoneId = zoneId };
            if (_campaign.GetAllAvailableCampaign(modelId, locationObj, sourceId, applicationId) != null)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet, Route("api/campaigns/leadinfo")]
        public IHttpActionResult GetCampaignLeadInfo(int leadId)
        {
            if (!RegExValidations.IsPositiveNumber(CustomParser.parseStringObject(leadId)))
            {
                return BadRequest();
            }
            else
            {
                var leadInfo = _campaign.GetCampaignLeadInfo(leadId);
                if (leadInfo == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(leadInfo);
                }
            }
        }

        [HttpGet, Route("api/campaign")]
        public IHttpActionResult GetCampaign(int modelId, int cityId, int platformId, int zoneId = 0, int campaignId = 0)
        {
            if (!RegExValidations.IsPositiveNumber(CustomParser.parseStringObject(modelId))
               || !RegExValidations.IsPositiveNumber(CustomParser.parseStringObject(platformId)))
            {
                return BadRequest();
            }

            var locationObj = new Location { CityId = cityId, ZoneId = zoneId };
            var campaign = _campaign.GetCampaignByCarLocation(modelId, locationObj, platformId, false, (int)Application.CarWale, campaignId);

            if (!CampaignValidation.IsCampaignValid(campaign))
            {
                return NotFound();
            }

            campaign = _campaign.GetCampaignWithScore(campaign, HttpContext.Current.Request, modelId, cityId, zoneId, platformId);

            try
            {
                var campaignDto = Mapper.Map<Campaign, CampaignDTO>(campaign);
                var dealerDetails = _newCarDealers.GetCampaignDealerDetails(campaign.Type, modelId,
                    campaign.DealerId, campaign.Id, cityId);

                if (dealerDetails != null)
                {
                    campaignDto.DealerShowroom = Mapper.Map<DealerDetails, DealersDTO>(dealerDetails);
                }
                else
                {
                    campaignDto.DealerShowroom = null;
                }

                return Ok(campaignDto);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignController.GetCampaign1()");
                objErr.LogException();

                try
                {
                    var campaignDto = Mapper.Map<Campaign, CampaignDTO>(campaign);
                    return Ok(campaignDto);
                }
                catch (Exception er)
                {
                    ExceptionHandler objError = new ExceptionHandler(er, "CampaignController.GetCampaign2()");
                    objError.LogException();
                    return NotFound();
                }
            }
        }

        [HttpGet, Route("api/campaign/template/{campaignId}/")]
        public IHttpActionResult GetCampaignTemplate([FromUri] int campaignId, string pageId, int sourceId)
        {
            try
            {
                Campaign campaign = _campaign.GetCampaignDetails(campaignId);
                return Ok(_campaignTemplate.GetCampaignTemplate(campaign, pageId, sourceId));
            }
            catch (Exception e)
            {
                ExceptionHandler objError = new ExceptionHandler(e, "CampaignController.GetCampaignTemplate()");
                objError.LogException();
                return NotFound();
            }
        }

        /// <summary>
        /// This function updates the IsRunning status of campaign i.e Active or Paused
        /// Created: Chenat Thambad, 02/01/2017
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("api/campaigns/{id}/runningstatus"), ApiAuthorization]
        public IHttpActionResult UpdateCampaignRunningStatus([FromUri] int id, [FromBody] CampaignStatus status)
        {
            if(status == null)
            {
                return BadRequest();
            }
            Logger.LogInfo("0> API hit for campaign = " + id + " Status = " + status.Status);
            if (_campaign.ChangeCampaignRunningStatus(id, status))
             {
                 return Ok();
             }
            else
             {
                 return BadRequest("Rejected");
             }            
        }

        [HttpGet, Route("api/campaign/{campaignId}/cvldetails/")]
        public IHttpActionResult GetCampaignCvlDetails([FromUri] int campaignId)
        {
            try
            {
                return Ok(_campaign.GetCampaignCvlDetails(campaignId));
            }
            catch (Exception e)
            {
                ExceptionHandler objError = new ExceptionHandler(e, "CampaignController.GetCampaignCvlDetails()");
                objError.LogException();
                return NotFound();
            }
        }

        [EnableCors(origins: "http://test.cartrade.com,https://test.cartrade.com,http://testm.cartrade.com,"
            + "https://testm.cartrade.com,http://testapi.cartrade.com,https://testapi.cartrade.com,http://www.cartrade.com,https://www.cartrade.com,"
            + "http://m.cartrade.com,https://m.cartrade.com,http://api.cartrade.com,https://api.cartrade.com", headers: "*", methods: "GET")]
        [HttpGet, Route("api/campaign/{campaignId}/cities/")]
        public IHttpActionResult GetCampaignCities([FromUri] int campaignId = -1, int modelId = -1)
        {
            try
            {
                if (campaignId < 1 || modelId < 1)
                {
                    return BadRequest("Invalid parameters");
                }

                return Ok(_campaign.GetCampaignCities(campaignId, modelId));
            }
            catch (Exception e)
            {
                ExceptionHandler objError = new ExceptionHandler(e, "CampaignController.GetCampaignCities()");
                objError.LogException();
                return InternalServerError();
            }
        }

        /// <summary>
        /// This function fetches the all Running campaigns for given car location
        /// Created: Saket Thapliyal, 18/04/2018
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/v3/campaigns/")]
        public IHttpActionResult GetCampaignsV3(int modelId, int cityId, int platformId, int applicationId, int areaId = 0, bool isDealerLocator = false, bool dealerAdminFilter = false)
        {
            try
            {
                if (modelId < 1 || cityId < 1 || platformId < 1 || applicationId < 1 || areaId < 0)
                {
                    return BadRequest();
                }

                var locationObj = new Location { CityId = cityId, AreaId = areaId };

                if (areaId > 0 && !_campaign.ValidateLocationOnArea(locationObj))
                    return BadRequest();

                var campaigns = Mapper.Map<List<DealerAd>, List<DealerAdDTO>>(_campaign.GetAllRunningCampaigns(modelId, locationObj, platformId, applicationId, isDealerLocator, dealerAdminFilter));
                return Ok(campaigns);
            }
            catch (Exception err)
            {
                Logger.LogException(err, HttpContext.Current.Request.ServerVariables["URL"]);
                return InternalServerError();
            }
        }
    }
}
