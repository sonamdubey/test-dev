using Bikewale.DTO.Dealer;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Dealer;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Dealer
{
    /// <summary>
    /// To Get List of Dealers
    /// Author : Sushil Kumar
    /// Created On : 7th October 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// Modified by :   Sumit Kate on 20 May 2016
    /// Description :   v2 API is created
    /// </summary>
    public class DealersListController : CompressionApiController//ApiController
    {
        private readonly IDealer _dealer = null;
		private readonly IDealerRepository _dealerRepo = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dealer"></param>
		public DealersListController(IDealer dealer, IDealerRepository dealerRepo)
        {
            _dealer = dealer;
			_dealerRepo = dealerRepo;
        }

        #region Dealer List for clients
        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 7th October 2015
        /// Get list of all dealers with details for a given make and city
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <param name="clientId"></param>
        /// <returns>List of dealers of paticular make in city</returns>
        [ResponseType(typeof(NewBikeDealerList)), Route("api/dealers/make/{makeId}/city/{cityId}/")]
        public IHttpActionResult Get(int makeId, int cityId, EnumNewBikeDealerClient? clientId = null)
        {
            IEnumerable<NewBikeDealerEntityBase> objDealers = null;
            NewBikeDealerList objDTODealerList = null;
            try
            {
                objDealers = _dealer.GetNewBikeDealersList(makeId, cityId, clientId);

                if (objDealers != null && objDealers.Any())
                {
                    objDTODealerList = new NewBikeDealerList();
                    objDTODealerList.Dealers = DealerListMapper.Convert(objDealers);
                    objDTODealerList.TotalDealers = objDealers.Count();

                    objDealers = null;

                    return Ok(objDTODealerList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.Dealer.DealersListController.Get");
               
                return InternalServerError();
            }
        }   // Get Dealers 

        /// <summary>
        /// Created by  :   Sumit Kate on 20 May 2016
        /// Description :   It returns Dealers list based on subscription model
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.Dealer.v2.NewBikeDealerList)), Route("api/v2/dealers/make/{makeId}/city/{cityId}/")]
        public IHttpActionResult GetV2(uint makeId, uint cityId)
        {
            if (makeId > 0 && cityId > 0)
            {
                try
                {
                    Bikewale.DTO.Dealer.v2.NewBikeDealerList dealers = null;
                    DealersEntity _dealers = null;
                    _dealers = _dealer.GetDealerByMakeCity(cityId, makeId);

                    if (_dealers != null && _dealers.Dealers.Any())
                    {
                        dealers = new DTO.Dealer.v2.NewBikeDealerList();
                        dealers.Dealers = DealerListMapper.ConvertV2(_dealers.Dealers);
                        return Ok(dealers);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.Dealer.DealersListController.GetV2");
                   
                    return InternalServerError();
                }
            }
            else
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Created By : Prabhu Puredla on 20 july 2018
        /// Description : Api to fetch dealers list based on model and city
        /// Modified by : Pratibha Verma on 14 August 2018
        /// Description : Added method call to filter out multioutlet dealers
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="dealerId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [Route("api/dealers/model/{modelId}/city/{cityId}/dealerId/{dealerId}/")]
		public IHttpActionResult GetMLADealers(uint modelId, uint cityId, uint dealerId, uint? areaId)
		{
			if (modelId > 0 && cityId > 0)
			{
				try
				{
                    IEnumerable<SecondaryDealerBase> secondaryDealers = _dealerRepo.GetMLADealers(modelId, dealerId, cityId, areaId);
                    List<SecondaryDealerBase> mlaDealers = null;
                    if (secondaryDealers != null)
                    {
                        mlaDealers = _dealer.FilterMultioutletDealers(secondaryDealers);
                        if (mlaDealers != null)
                        {
                            return Ok(mlaDealers);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }
				catch (Exception ex)
				{
					ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.Dealer.DealersListController.GetMLADealers");

					return InternalServerError();
				}
			}
			else
			{
				return BadRequest();
			}
		}

        /// <summary>
        /// Created by  : Pratibha Verma on 31 August 2018
        /// Description : to get dealers list bay makeId and cityId
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [Route("api/dealers/all/make/{makeId}/city/{cityId}/")]
        public IHttpActionResult GetAllDealersbyMakeCity(uint makeId, uint cityId)
        {
            if (makeId > 0 && cityId > 0)
            {
                IEnumerable<NewBikeDealerDetails> objDealer = _dealerRepo.GetAllDealersByMakeCity(makeId, cityId);
                if (objDealer != null && objDealer.Any())
                {
                    return Ok(objDealer);
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

        #endregion
    }
}
