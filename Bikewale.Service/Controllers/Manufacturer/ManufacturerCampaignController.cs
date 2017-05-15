using Bikewale.DTO;
using Bikewale.Entities;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Manufacturer
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 10-May-2017
    /// Summary: Controller which holds actions for manufacturer's campaign
    /// </summary>
    public class ManufacturerCampaignController : CompressionApiController
    {
        private readonly IPriceQuote _objIPQ = null;
        private readonly IPriceQuoteCache _objPQCache = null;

        public ManufacturerCampaignController()
        {

        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objDealer"></param>
        public ManufacturerCampaignController(IPriceQuote objIPQ, IPriceQuoteCache objPQCache)
        {
            _objIPQ = objIPQ;
            _objPQCache = objPQCache;
        }
        /// <summary>
        /// Returns the list of Dealer
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ManufactureDealerDTO>))]
        public IHttpActionResult Get(uint city)
        {
            IEnumerable<ManufactureDealer> objDealers = null;
            IEnumerable<ManufactureDealerDTO> dealersData = null;
            try
            {
                objDealers = _objPQCache.GetManufacturerDealers(city);
                if (objDealers != null && objDealers.Count() > 0)
                {
                    dealersData = ManufacturingCampaign.Convert(objDealers);
                    return Ok(dealersData);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Service.Controllers.Manufacturer.Get()");
                return InternalServerError();
            }
        }
    }
}