using Carwale.Entity.Blocking;
using Carwale.Interfaces.Blocking;
using Carwale.Service.Filters;
using Carwale.Utility;
using System.Linq;
using System.Web.Http;

namespace Carwale.Service.Controllers.Blocking
{
    public class BlockedIpsController : ApiController
    {
        private readonly IBlockIPRepository _blockIPRepository;
        public BlockedIpsController(IBlockIPRepository blockIPRepository)
        {
            _blockIPRepository = blockIPRepository;
        }

        [ApiAuthorization, HandleException, LogRequest]
        public IHttpActionResult Put([FromBody]BlockIPInputs inputs)
        {
            if (inputs == null)
            {
                ModelState.AddModelError("Input", "Request body should not be null");
            }
            string addedBy = HttpContextUtils.GetHeader<string>("source");
            if (string.IsNullOrEmpty(addedBy))
            {
                ModelState.AddModelError("Header", "'source' Header missing.");
            }
            else if (addedBy.Length > 30)
            {
                ModelState.AddModelError("Header", "Invalid Header.('source' header length must be less or equal to 30 character.)");
            }
            if (ModelState.IsValid)
            {
                inputs.IpAddresses = inputs.IpAddresses.Distinct();
                _blockIPRepository.BlockIpAddresses(inputs.IpAddresses, inputs.Reason, addedBy);
                return Ok("IPs successfully blocked.");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [ApiAuthorization, HandleException, LogRequest]
        public IHttpActionResult Delete([FromBody]BlockIPInputs inputs)
        {
            if (inputs == null)
            {
                ModelState.AddModelError("Input", "Request body should not be null");
            }
            string addedBy = HttpContextUtils.GetHeader<string>("source");
            if (string.IsNullOrEmpty(addedBy))
            {
                ModelState.AddModelError("Header", "'source' Header missing.");
            }
            else if (addedBy.Length > 30)
            {
                ModelState.AddModelError("Header", "Invalid Header.('source' header length must be less or equal to 30 character.)");
            }
            if (ModelState.IsValid)
            {
                inputs.IpAddresses = inputs.IpAddresses.Distinct();
                _blockIPRepository.UnblockIpAddresses(inputs.IpAddresses);
                return Ok("IPs successfully unblocked.");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
