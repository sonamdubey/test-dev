using Bikewale.DTO.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.BikeData;
using System.Collections.Generic;
using System.Web.Http;

namespace Bikewale.Service.Controllers.BikeData
{
    public class VersionsController : ApiController
    {
        private readonly IBikeVersions<BikeVersionEntity, uint> _versionsRepository;
        /// <summary>
        /// Constructor to Initialize cache layer
        /// </summary>
        /// <param name="versionsRepository"></param>
        public VersionsController(IBikeVersions<BikeVersionEntity, uint> versionsRepository)
        {
            _versionsRepository = versionsRepository;
        }

        /// <summary>
        /// Created by sajal gupta on 23-05-2017 to get version segmets details
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/userprofiles/models/")]
        public IHttpActionResult Get()
        {
            try
            {
                IEnumerable<BikeModelVersionsDetails> objVersionsEntity = _versionsRepository.GetModelVersions();

                IEnumerable<ModelSpecificationsDTO> objVersionsDTO = MakeModelEntityMapper.Convert(objVersionsEntity);

                return Ok(objVersionsDTO);
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "MakeModelsController.Get");
                return InternalServerError();
            }
        }
    }
}
