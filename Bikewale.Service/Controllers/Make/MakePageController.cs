using Bikewale.DTO.Make;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Make;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Make
{
    /// <summary>
    /// Make Page Controller
    /// Author  :   Sumit Kate
    /// Created :   03 Sept 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class MakePageController : CompressionApiController//ApiController
    {
        private readonly IBikeMakes<BikeMakeEntity, int> _makesRepository;
        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="makesRepository"></param>
        /// <param name="modelRepository"></param>
        public MakePageController(IBikeMakes<BikeMakeEntity, int> makesRepository, IBikeModelsRepository<BikeModelEntity, int> modelRepository)
        {
            _makesRepository = makesRepository;
            _modelRepository = modelRepository;
        }

        /// <summary>
        /// Returns the Make page data which includes
        /// Make name, Description(small and large) and popular bikes
        /// </summary>
        /// <param name="makeId">make id</param>
        /// <param name="totalBikeCount">total popular bike count</param>
        /// <returns></returns>
        [ResponseType(typeof(MakePage))]
        public IHttpActionResult Get(int makeId)
        {
            BikeMakePageEntity entity = null;
            BikeDescriptionEntity description = null;
            IEnumerable<MostPopularBikesBase> objModelList = null;
            MakePage makePage = null;

            try
            {
                objModelList = _modelRepository.GetMostPopularBikesByMake(makeId);
                description = _makesRepository.GetMakeDescription(makeId);

                if (objModelList != null && objModelList.Any() && description != null)
                {
                    entity = new BikeMakePageEntity();
                    entity.Description = description;
                    entity.PopularBikes = objModelList;
                    makePage = MakePageEntityMapper.Convert(entity);

                    objModelList = null;

                    return Ok(makePage);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.Make.MakePageController");
                return InternalServerError();
            }
            return NotFound();
        }

    }
}
