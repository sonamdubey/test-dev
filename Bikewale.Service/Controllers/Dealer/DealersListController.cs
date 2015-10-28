using Bikewale.DTO.Dealer;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Dealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Dealer
{
    /// <summary>
    /// To Get List of Dealers
    /// Author : Sushil Kumar
    /// Created On : 7th October 2015
    /// </summary>
    public class DealersListController : ApiController
    {
        private readonly IDealer _dealer = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dealer"></param>
        public DealersListController(IDealer dealer)
        {
            _dealer = dealer;
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

                if (objDealers != null && objDealers.Count() > 0)
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.City.CityListController");
                objErr.SendMail();
                return InternalServerError();
            }
        }   // Get Dealers 
        #endregion
    }
}
