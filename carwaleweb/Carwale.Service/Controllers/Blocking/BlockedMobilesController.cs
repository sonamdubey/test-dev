using Carwale.Entity.Blocking;
using Carwale.Entity.Blocking.Enums;
using Carwale.Interfaces.Blocking;
using Carwale.Service.Filters;
using Carwale.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Carwale.Service.Controllers.Blocking
{
    public class BlockedMobilesController : ApiController
    {
        private readonly IBlockMobileRepository _blockedMobileRepo;
        private readonly IBlockedCommunicationsRepository _blockedCommunicationRepo;
        public BlockedMobilesController(IBlockMobileRepository blockedMobileRepo, IBlockedCommunicationsRepository blockedCommunicationRepo)
        {
            _blockedMobileRepo = blockedMobileRepo;
            _blockedCommunicationRepo = blockedCommunicationRepo;
        }

        [ApiAuthorization, HandleException, LogRequest]
        public IHttpActionResult Put([FromBody]BlockMobileInputs inputs)
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
                inputs.MobileNos = inputs.MobileNos.Distinct();
                _blockedMobileRepo.BlockMobileNos(inputs.MobileNos, inputs.Reason, addedBy);
                _blockedCommunicationRepo.BlockCommunication(new BlockedCommunicationRequest { Communications = GetBlockedCommunicationRequest(inputs,addedBy) });
                return Ok("Mobile numbers successfully blocked.");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [ApiAuthorization, HandleException, LogRequest]
        public IHttpActionResult Delete([FromBody]BlockMobileInputs inputs)
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
                inputs.MobileNos = inputs.MobileNos.Distinct();
                _blockedMobileRepo.UnblockMobileNos(inputs.MobileNos);
                _blockedCommunicationRepo.UnblockCommunication(new BlockedCommunicationRequest { Communications = GetBlockedCommunicationRequest(inputs, addedBy) });
                return Ok("Mobile numbers successfully unblocked.");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        private List<BlockedCommunication> GetBlockedCommunicationRequest(BlockMobileInputs inputs, string addedBy)
        {
            List<BlockedCommunication> blockedCommunications = new List<BlockedCommunication>();
            foreach (var mobile in inputs.MobileNos)
            {
                blockedCommunications.Add(new BlockedCommunication
                {
                    Value = mobile,
                    Reason = inputs.Reason,
                    ActionBy = addedBy,
                    Type = CommunicationType.Mobile,
                    Module = CommunicationModule.All
                });
            }
            return blockedCommunications;
        }
    }
}
