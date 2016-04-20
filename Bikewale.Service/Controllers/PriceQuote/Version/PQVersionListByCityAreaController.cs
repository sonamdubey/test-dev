using Bikewale.BAL.PriceQuote;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.PriceQuote.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Model;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.Version
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 20 Apr 2016
    /// Summary: API to return PriceQuote for model by city and area
    /// </summary>
    public class PQVersionListByCityAreaController : ApiController
    {
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion = null;
        /// <summary>
        /// To Fetch PQ versionList, PQID and dealerId
        /// </summary>
        /// <param name="objVersion"></param>
        public PQVersionListByCityAreaController(IBikeVersions<BikeVersionEntity, uint> objVersion)
        {
            _objVersion = objVersion;
        }

        /// <summary>
        /// Gets the Version list for given model and city
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [ResponseType(typeof(PQByCityAreaEntity))]
        public IHttpActionResult Get(uint modelId, int? cityId = null, int? areaId = null)
        {
            if (cityId < 0 || modelId < 0)
            {
                return BadRequest();
            }
            List<BikeVersionMinSpecs> objVersionsList = null;
            PQByCityAreaDTO objPQDTO = null;
            PQByCityAreaEntity pqEntity = null;
            BikeModelsRepository<BikeModelEntity, int> modelMinSpec = new DAL.BikeData.BikeModelsRepository<BikeModelEntity, int>();
            objVersionsList = modelMinSpec.GetVersionMinSpecs((int)modelId, true);
            try
            {

                if (objVersionsList != null && objVersionsList.Count > 0)
                {
                    PQByCityArea pqByCityArea = new PQByCityArea();
                    pqEntity = pqByCityArea.GetVersionList((int)modelId, objVersionsList, cityId, areaId);
                    objPQDTO = ModelMapper.Convert(pqEntity);
                    objVersionsList.Clear();
                    objVersionsList = null;
                    return Ok(objPQDTO);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Version.PQVersionListByCityAreaController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}
