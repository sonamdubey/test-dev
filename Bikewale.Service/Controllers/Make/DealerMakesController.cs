using Bikewale.DTO.Make;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Make
{
    /// <summary>
    /// Author  :   Sumit Kate
    /// Created :   04 Sept 2015
    /// Dealer Makes Controller
    /// </summary>
    public class DealerMakesController : CompressionApiController//ApiController
    {
        private readonly IDealer _objDealer = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objDealer"></param>
        /// <param name="objDealerCache"></param>
        public DealerMakesController(IDealer objDealer, IDealerCacheRepository objDealerCache)
        {
            _objDealer = objDealer;
            _objDealerCache = objDealerCache;
        }

        /// <summary>
        /// Returns the Dealer make list
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<NewBikeDealersMakeBase>))]
        public IHttpActionResult Get()
        {
            IEnumerable<NewBikeDealersMakeEntity> objMakes = null;
            NewBikeDealersMakeList makes = null;
            try
            {
                objMakes = _objDealerCache.GetDealersMakesList();
                if (objMakes != null && objMakes.Any())
                {
                    makes = new NewBikeDealersMakeList();
                    makes.Makes = objMakes.Select(s => new NewBikeDealersMakeBase() { Text = s.MakeName, Value = s.MakeId.ToString(), MaskingName = s.MaskingName }).ToList();
                    objMakes = null;
                    return Ok(makes);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Service.Controllers.Make.DealerMakesController.Get");
               
                return InternalServerError();
            }
        }
    }
}
