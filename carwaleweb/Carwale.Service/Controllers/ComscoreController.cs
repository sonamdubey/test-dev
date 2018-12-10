using System.Web.Http;

namespace Carwale.Service.Controllers
{
    public class ComscoreController : ApiController
    {
        public IHttpActionResult GetDynamicPageViewKeyword()
        {
            return Ok("pageview_candidate");
        }
    }
}
