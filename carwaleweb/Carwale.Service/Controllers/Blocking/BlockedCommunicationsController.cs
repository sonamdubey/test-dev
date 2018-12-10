using AutoMapper;
using Carwale.DTOs.Blocking;
using Carwale.Entity.Blocking;
using Carwale.Interfaces.Blocking;
using Carwale.Service.Filters;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FluentValidation;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;

namespace Carwale.Service.Controllers.Blocking
{
    public class BlockedCommunicationsController: ApiController
    {
        private readonly IBlockedCommunicationsRepository _blockedCommunicationRepo;
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };
        public BlockedCommunicationsController(IBlockedCommunicationsRepository blockedCommunicationRepo)
        {
            _blockedCommunicationRepo = blockedCommunicationRepo;
        }

        [HandleException, LogRequest]
        [HttpGet, Route("api/blockedcommunication/")]
        public IHttpActionResult IsCommunicationBlocked([FromUri]BlockedCommunicationDto communicationDto)
        {
            
            if (communicationDto == null)
            {
                ModelState.AddModelError("Input", "Request body should not be null"); 
            }
            else
            {
                var validation = new BlockedCommunicationDtoValidator().Validate(communicationDto, ruleSet: "Common");
                if (!validation.IsValid)
                {
                    foreach (var error in validation.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            BlockedCommunication communication = Mapper.Map<BlockedCommunicationDto, BlockedCommunication>(communicationDto);
            bool isBlocked = _blockedCommunicationRepo.IsCommunicationBlocked(communication);

            if (isBlocked)
            {
                var resp = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent
                };
                return ResponseMessage(resp);
            }
            return NotFound();
        }

        [ApiAuthorization,HandleException, LogRequest]
        [HttpDelete, Route("api/blockedcommunication/")]
        public IHttpActionResult UnblockCommunication(IEnumerable<BlockedCommunicationDto> communicationDtos)
        {
            if (communicationDtos == null)
            {
                ModelState.AddModelError("Input", "Request body should not be null");
            }
            else
            {
                ValidateCommunicationDtos(communicationDtos, "Common,ActionBy");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            BlockedCommunicationRequest communicationRequest = new BlockedCommunicationRequest();
            communicationRequest.Communications = Mapper.Map<IEnumerable<BlockedCommunicationDto>, IEnumerable<BlockedCommunication>>(communicationDtos);
            communicationRequest = _blockedCommunicationRepo.UnblockCommunication(communicationRequest);
            var respDtos = Mapper.Map<IEnumerable<BlockedCommunication>, IEnumerable<BlockedCommunicationDto>>(communicationRequest.Communications);
            return Content(communicationRequest.IsAllSuccess ? System.Net.HttpStatusCode.OK : (System.Net.HttpStatusCode)207,
                respDtos, new JsonMediaTypeFormatter { SerializerSettings = _serializerSettings });
        }
        
        [ApiAuthorization, HandleException, LogRequest]
        [HttpPost, Route("api/blockedcommunication/")]
        public IHttpActionResult Post(IEnumerable<BlockedCommunicationDto> communicationDtos)
        {

            if (communicationDtos == null)
            {
                ModelState.AddModelError("Input", "Request body should not be null");
            }
            else
            {
                ValidateCommunicationDtos(communicationDtos, "Common, Reason, ActionBy");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            BlockedCommunicationRequest communicationRequest = new BlockedCommunicationRequest();
            communicationRequest.Communications = Mapper.Map<IEnumerable<BlockedCommunicationDto>, IEnumerable<BlockedCommunication>>(communicationDtos);
            communicationRequest = _blockedCommunicationRepo.BlockCommunication(communicationRequest);
            var respDtos = Mapper.Map<IEnumerable<BlockedCommunication>, IEnumerable<BlockedCommunicationDto>>(communicationRequest.Communications);
            return Content(communicationRequest.IsAllSuccess ? System.Net.HttpStatusCode.OK : (System.Net.HttpStatusCode)207,
                respDtos, new JsonMediaTypeFormatter { SerializerSettings = _serializerSettings });
        }

        private void ValidateCommunicationDtos(IEnumerable<BlockedCommunicationDto> communicationDtos,string rule)
        {
            foreach (var communicationDto in communicationDtos)
            {
                var validation = new BlockedCommunicationDtoValidator().Validate(communicationDto, ruleSet:rule);
                foreach (var error in validation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
        }
    }
}
