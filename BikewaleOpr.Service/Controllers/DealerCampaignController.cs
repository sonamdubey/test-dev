using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Interface.ContractCampaign;
using System.Collections.Generic;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers
{
    /// <summary>
    /// Created by  :   Sumit Kate on 18 Jan 2017
    /// Description :   Manage Dealer Campaign Controller
    /// </summary>
    public class DealerCampaignController : ApiController
    {
        private readonly IDealerCampaignRepository _objDealerCampaignRepository;
        public DealerCampaignController(IDealerCampaignRepository objDealerCampaignRepository)
        {
            _objDealerCampaignRepository = objDealerCampaignRepository;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 18 Jan 2017
        /// Description :   Returns Dealer's Make list by city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpGet, Route("api/dealermakes/")]
        public IHttpActionResult MakesByDealerCity(uint cityId)
        {
            if (cityId > 0)
            {
                IEnumerable<BikeMakeEntityBase> makes = null;
                makes = _objDealerCampaignRepository.MakesByDealerCity(cityId);
                if (makes != null)
                {
                    return Ok(makes);
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

        /// <summary>
        /// Created by  :   Sumit Kate on 18 Jan 2017
        /// Description :   Returns Dealers list by city and makeid
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="activecontract"></param>
        /// <returns></returns>
        [HttpGet, Route("api/dealers/city/{cityId}/make/{makeId}/")]
        public IHttpActionResult DealersByMakeCity(uint cityId, uint makeId, bool activecontract = false)
        {
            if (cityId > 0 && makeId > 0)
            {
                IEnumerable<DealerEntityBase> dealers = null;
                dealers = _objDealerCampaignRepository.DealersByMakeCity(cityId, makeId, activecontract);
                if (dealers != null)
                {
                    return Ok(dealers);
                }
                else { return NotFound(); }
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 18 Jan 2017
        /// Description :   Returns Dealer Campaigns
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="activecontract"></param>
        /// <returns></returns>
        [HttpGet, Route("api/dealercampaigns/")]
        public IHttpActionResult DealerCampaigns(uint dealerId, bool activecontract = false)
        {
            if (dealerId > 0)
            {
                IEnumerable<DealerCampaignDetailsEntity> campaigns = null;
                campaigns = _objDealerCampaignRepository.DealerCampaigns(dealerId, activecontract);
                if (campaigns != null)
                {
                    return Ok(campaigns);
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
    }
}
