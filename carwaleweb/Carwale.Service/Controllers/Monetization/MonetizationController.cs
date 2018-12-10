using Carwale.Interfaces.Monetization;
using System.Web.Http;
using Carwale.Utility;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Dealers.URI;
using Carwale.Service.Filters.Dealer;
using AutoMapper;

namespace Carwale.Service.Controllers.Monetization
{
    public class MonetizationController : ApiController
    {
        protected readonly IMonetization _monetization;
        public MonetizationController(IMonetization monetization)
        {
            _monetization = monetization;
        }

        [HttpGet,Route("api/monetization/ads/")]
        public IHttpActionResult ModelAddUnits(int modelId, int cityId,string screenType,string zoneId="")
        {
            var platform = Request.Headers.GetValueFromHttpHeader<int>("sourceid");

            return Ok(_monetization.ModelAddUnits(modelId, cityId, zoneId, platform, screenType));
        }

        [HttpGet, Route("api/v1/monetization/ads/")]
        [DealerAdValidation]
        public IHttpActionResult ModelAddUnits([FromUri] CampaignInputURI input)
        {
            Location locationObj = Mapper.Map<CampaignInputURI, Location>(input);
            int platformId = Request.Headers.GetValueFromHttpHeader<int>("sourceid");
            return Ok(_monetization.ModelAddUnitsV1(input.ModelId, locationObj, platformId, input.ScreenType, input.CampaignId));
        }
    }
}
