using Bikewale.DTO.PriceQuote.Model;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote.Model;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.Model
{
    /// <summary>
    /// Price Quote Model List Controller
    /// Author  :   Sumit Kate
    /// Created on  :   20 Aug 2015
    /// </summary>
    public class PQModelListController : ApiController
    {
        private readonly IBikeModels<BikeModelEntity, int> _modelsRepository = null;

        public PQModelListController(IBikeModels<BikeModelEntity, int> modelsRepository)
        {
            _modelsRepository = modelsRepository;
        }
        /// <summary>
        /// Returns the List of Models of a Model
        /// </summary>
        /// <param name="makeId">Bike Make</param>
        /// <returns></returns>
        [ResponseType(typeof(PQModelList))]
        public IHttpActionResult Get(int makeId)
        {
            List<BikeModelEntityBase> objModelList = null;
            PQModelList objDTOModelList = null;
            try
            {
                objModelList = _modelsRepository.GetModelsByType(EnumBikeType.PriceQuote, makeId);

                if (objModelList != null && objModelList.Count > 0)
                {
                    // Auto map the properties
                    objDTOModelList = new PQModelList();
                    objDTOModelList.Models = PQModelListMapper.Convert(objModelList);

                    objModelList.Clear();
                    objModelList = null;

                    return Ok(objDTOModelList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Model.PQModelListController.Get");
               
                return InternalServerError();
            }
        }
    }
}
