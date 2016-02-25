using Bikewale.DTO.Make;
using Bikewale.Entities.Dealer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Make
{
    /// <summary>
    /// Author  :   Sumit Kate
    /// Created :   04 Sept 2015
    /// Dealer Makes Controller
    /// </summary>
    public class DealerMakesController : ApiController
    {
        private readonly IDealer _objDealer = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objDealer"></param>
        public DealerMakesController(IDealer objDealer)
        {
            _objDealer = objDealer;
        }

        /// <summary>
        /// Returns the Dealer make list
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<NewBikeDealersMakeBase>))]
        public IHttpActionResult Get()
        {
            IList<NewBikeDealersMakeEntity> objMakes = null;
            NewBikeDealersMakeList makes = null;
            try
            {
                objMakes = _objDealer.GetDealersMakesList();
                if (objMakes != null && objMakes.Count > 0)
                {
                    makes = new NewBikeDealersMakeList();
                    makes.Makes = objMakes.Select(s => new NewBikeDealersMakeBase() { Text = s.MakeName, Value = s.MakeId.ToString() }).ToList();

                    objMakes.Clear();
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
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Service.Controllers.Make.DealerMakesController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
