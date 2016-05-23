using Bikewale.BAL.PriceQuote;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.PriceQuote.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Model;
using Bikewale.Service.Utilities;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.Version
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 20 Apr 2016
    /// Summary: API to return PriceQuote for model by city and area
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class PQVersionListByCityAreaController : CompressionApiController//ApiController
    {
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion = null;
        private readonly IBikeModelsRepository<BikeModelEntity, int> _objModel = null;

        /// <summary>
        /// To Fetch PQ versionList, PQID and dealerId
        /// </summary>
        /// <param name="objVersion"></param>
        /// <param name="objModel"></param>
        public PQVersionListByCityAreaController(IBikeVersions<BikeVersionEntity, uint> objVersion, IBikeModelsRepository<BikeModelEntity, int> objModel)
        {
            _objVersion = objVersion;
            _objModel = objModel;
        }

        /// <summary>
        /// Gets the Version list for given model and city
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [ResponseType(typeof(PQByCityAreaEntity)), Route("api/model/versionlistprice/")]
        public IHttpActionResult Get(int modelId, int? cityId = null, int? areaId = null, string deviceId = null)
        {
            if (cityId < 0 || modelId < 0)
            {
                return BadRequest();
            }
            IEnumerable<BikeVersionMinSpecs> objVersionsList = null;
            PQByCityAreaDTO objPQDTO = null;
            PQByCityAreaEntity pqEntity = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                IBikeModelsRepository<BikeModelEntity, int> objVersion = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();
                objVersionsList = objVersion.GetVersionMinSpecs(modelId, true);
            }

            try
            {
                if (objVersionsList != null && objVersionsList.Count() > 0)
                {
                    PQByCityArea pqByCityArea = new PQByCityArea();
                    string platformId = string.Empty;
                    UInt16 platform = default(UInt16);
                    if (Request.Headers.Contains("platformId"))
                    {
                        platformId = Request.Headers.GetValues("platformId").First().ToString();
                    }
                    UInt16.TryParse(platformId, out platform);
                    pqEntity = pqByCityArea.GetVersionList(modelId, objVersionsList, cityId, areaId, platform, null, null, deviceId);
                    objPQDTO = ModelMapper.Convert(pqEntity);
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
