using Carwale.BL.Classified;
using Carwale.DAL.Classified;
using Carwale.Interfaces.Classified;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Carwale.Service.Controllers.Classified
{
   public class CWCTCityMappingController : ApiController
    {
       private ICWCTCityMappingRepositiory _cwctMappingRepo;

       public CWCTCityMappingController(ICWCTCityMappingRepositiory cwctMappingRepo)
        {
            _cwctMappingRepo = cwctMappingRepo;
        }

        [HttpGet]
        [ActionName("iscartradecity")]
        public IHttpActionResult IsCartradeCity(string cityId)
        {
            int cwCityId = 0;
            try
            {
                if (Int32.TryParse(cityId, out cwCityId))
                {
                    if(cwCityId > 0)
                        return Ok(_cwctMappingRepo.IsCarTradeCity(cwCityId));
                    return BadRequest("Invalid City Id");
                }
                return BadRequest("Invalid City Id");
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetResultsWithFiltersAndPager()");
                objErr.LogException();
                return InternalServerError(ex);
            }
            
        }
    }
}
