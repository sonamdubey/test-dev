using Carwale.BL.GeoLocation;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Leads;
using Carwale.Interfaces.Leads;
using Carwale.Service.Filters;
using Carwale.Utility;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace Carwale.Service.Controllers.Leads
{
    public class DealerInquiriesController : ApiController
    {
         private readonly IDealerInquiry _dealerInquiry;

         public DealerInquiriesController(IDealerInquiry dealerInquiry)
        {
            _dealerInquiry = dealerInquiry;
        }

        [HttpPost, Route("api/dealer/inquiries/"), HandleException]
        public IHttpActionResult DealerLead([FromBody] DealerInquiry dealerInquiry)
        {
            var response = new HttpResponseMessage();
            if (dealerInquiry != null)
            {
                dealerInquiry.UserLocation = new ElasticLocation().FormCompleteLocation(dealerInquiry.UserLocation);

                List<ulong> _pqLeadIds = _dealerInquiry.DealerInquiries(dealerInquiry);
                List<string> _enryptIds = new List<string>();

                foreach (var id in _pqLeadIds)
                {
                    _enryptIds.Add(CarwaleSecurity.Encrypt(CustomParser.parseStringObject(id)));
                }

                response.Content = new StringContent(_enryptIds.ToDelimatedString(','));
            }
            return ResponseMessage(response);
        }
    }
}
