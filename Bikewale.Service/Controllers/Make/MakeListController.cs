using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.Make;
using AutoMapper;
using System.Web.Http.Description;
using Bikewale.Service.AutoMappers.Make;
using Bikewale.Notifications;


namespace Bikewale.Service.Controllers.Make
{
    /// <summary>
    /// To Get List of Makes 
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class MakeListController : ApiController
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
