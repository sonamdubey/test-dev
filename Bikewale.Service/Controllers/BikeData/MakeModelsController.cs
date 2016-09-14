using Bikewale.DTO.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.BikeData;
using Bikewale.Service.Utilities;
using System.Collections.Generic;
using System.Web.Http;

namespace Bikewale.Service.Controllers.BikeData
{
    /// <summary>
    /// Created by  :   Sumit Kate on 13 Sep 2016
    /// Description :   Make Models Controller
    /// </summary>
    public class MakeModelsController : CompressionApiController
    {
        private readonly IBikeMakesCacheRepository<int> _makesRepository;
        /// <summary>
        /// Constructor to Initialize cache layer
        /// </summary>
        /// <param name="makesRepository"></param>
        public MakeModelsController(IBikeMakesCacheRepository<int> makesRepository)
        {
            _makesRepository = makesRepository;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 13 Sep 2016
        /// Description :   Gets All the makes and their models by calling cache layer
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            IEnumerable<MakeModelBase> makeModels = null;
            try
            {
                makeModels = MakeModelEntityMapper.Convert(_makesRepository.GetAllMakeModels());
            }
            catch (System.Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "MakeModelsController.Get");
                objErr.SendMail();
                InternalServerError();
            }
            return Ok(makeModels);
        }
    }
}
