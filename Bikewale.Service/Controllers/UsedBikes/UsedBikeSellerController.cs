using Bikewale.DTO.UsedBikes;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Bikewale.Service.Controllers.UsedBikes
{
    public class UsedBikeSellerController : ApiController
    {
        [Route("api/used/sell/listing/")]
        public IHttpActionResult Post([FromBody]SellBikeAdDTO ad)
        {

            if (ModelState.IsValid)
            {
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
