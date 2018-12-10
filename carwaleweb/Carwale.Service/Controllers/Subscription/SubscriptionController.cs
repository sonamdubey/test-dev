using Carwale.Entity.Subscription;
using Carwale.Interfaces.Subscription;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Carwale.Service.Controllers.Subscription
{
    public class SubscriptionController : ApiController
    {
        private readonly ISubscriptionRepository _subscribeRepo;

        public SubscriptionController(ISubscriptionRepository subscribeRepo)
        {
            _subscribeRepo = subscribeRepo;
        }

        [EnableCors(origins: "https://www.bikewale.com,https://staging.bikewale.com,http://localhost:9096", headers: "*", methods: "POST")]
        [HttpPost, Route("api/subscribe/")]
        public IHttpActionResult Subscribe([FromBody] SubscriptionContent subContent)
        {
            string strRegex = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$";
            Regex re = new Regex(strRegex);

            if (subContent == null || string.IsNullOrEmpty(subContent.Email) || !re.IsMatch(subContent.Email) || subContent.Category <= 0)
            {
                return BadRequest("Incorrect Input!");
            }
            else
            {
                if (subContent.Type <= 0)
                    subContent.Type = -1;
                return Ok(_subscribeRepo.Subscribe(subContent.Email, subContent.Category, subContent.Type));
            }
        }
    }
}

