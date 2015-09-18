using AutoMapper;
using Bikewale.DAL.BikeBooking;
using Bikewale.DTO.BikeBooking.Model;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Bikebooking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Bikewale.Interfaces.BikeData;

namespace Bikewale.Service.Controllers.BikeBooking.Model
{
    /// <summary>
    /// BikeBooking Model List controller
    /// Author  :   Sumit Kate
    /// Created On  :   20 Aug 2015
    /// </summary>
    public class BBModelListController : ApiController
    {
        private readonly IBikeModelsRepository<BikeModelEntity, int> _repository = null;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public BBModelListController(IBikeModelsRepository<BikeModelEntity, int> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// Returns the Model List for a make
        /// </summary>
        /// <param name="makeId">Make Id</param>
        /// <returns></returns>
        [ResponseType(typeof(BBModelList))]
        public IHttpActionResult Get(UInt16 makeId)
        {
            IEnumerable<BikeModelEntityBase> objModelList = null;
            BBModelList objDTOModelList = null;
            try
            {
                objModelList = _repository.GetModelsByType(EnumBikeType.PriceQuote, makeId);

                if (objModelList != null)
                {
                    objDTOModelList = new BBModelList();
                    objDTOModelList.Models = BBModelListMapper.Convert(objModelList);

                    return Ok(objDTOModelList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.BikeBooking.Model.BBModelListController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
