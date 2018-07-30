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
    /// To Get List of Makes 
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class MakeListController : CompressionApiController//ApiController
    {
        //private readonly IBikeMakes<BikeMakeEntity, int> _makesRepository;
        private readonly IBikeMakesCacheRepository _makesRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="makesRepository"></param>
        public MakeListController(IBikeMakesCacheRepository makesRepository)
        {
            _makesRepository = makesRepository;
        }

        #region Makes List
        /// <summary>
        ///  To get List of Makes based on request Type 
        /// </summary>
        /// <param name="requestType"></param>
        /// <returns>Makes List</returns>
        [ResponseType(typeof(MakeList))]
        public IHttpActionResult Get(EnumBikeType requestType)
        {
            IEnumerable<BikeMakeEntityBase> objMakeList = null;
            MakeList objDTOMakeList = null;
            try
            {
                objMakeList = _makesRepository.GetMakesByType(requestType);

                if (objMakeList != null && objMakeList.Any())
                {
                    objDTOMakeList = new MakeList();

                    objDTOMakeList.Makes = MakeListMapper.Convert(objMakeList);

                    objMakeList = null;

                    return Ok(objDTOMakeList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Make.MakeController");
               
                return InternalServerError();
            }
            return NotFound();
        }   // Get 
        #endregion


    }    // Class
}   // Namespace
