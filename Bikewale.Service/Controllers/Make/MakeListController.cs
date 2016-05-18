using Bikewale.DTO.Make;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Make;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
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
        private readonly IBikeMakes<BikeMakeEntity, int> _makesRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="makesRepository"></param>
        public MakeListController(IBikeMakes<BikeMakeEntity, int> makesRepository)
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
            List<BikeMakeEntityBase> objMakeList = null;
            MakeList objDTOMakeList = null;
            try
            {
                objMakeList = _makesRepository.GetMakesByType(requestType);

                if (objMakeList != null && objMakeList.Count > 0)
                {
                    objDTOMakeList = new MakeList();

                    objDTOMakeList.Makes = MakeListMapper.Convert(objMakeList);

                    objMakeList.Clear();
                    objMakeList = null;

                    return Ok(objDTOMakeList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Make.MakeController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }   // Get 
        #endregion


    }    // Class
}   // Namespace
