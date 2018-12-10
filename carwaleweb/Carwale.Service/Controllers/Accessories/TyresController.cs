using AutoMapper;
using Carwale.DTOs.Accessories.Tyres;
using Carwale.Entity.Accessories.Tyres;
using Carwale.Interfaces.Accessories.Tyres;
using Carwale.Notifications.Logs;
using System;
using System.Web.Http;

namespace Carwale.Service.Controllers.Accessories
{
    public class TyresController : ApiController
    {
        private readonly ITyresBL _tyresBL;
        private readonly ITyresRepository _tyresRepo;

        public TyresController(ITyresBL tyresBL,ITyresRepository tyresRepo)
        {
            _tyresBL = tyresBL;
            _tyresRepo=tyresRepo;
        }

        [HttpGet,Route("api/models/{modelIds:regex(^(\\d+(,\\d+)*)?$)}/tyres/")]
        public IHttpActionResult GetTyresByCarModel(string modelIds,int pageNo,int pageSize=10) 
        {
            try
            {
                var modelTyres = _tyresBL.GetTyresByCarModels(modelIds, pageNo, pageSize);

                return Ok(Mapper.Map<TyreList, TyreListDTO>(modelTyres));
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [HttpGet, Route("api/versions/{versionId:int:min(1)}/tyres/")]
        public IHttpActionResult GetTyresByCarVersion(int versionId, int pageNo, int pageSize = 10)
        {
            try
            {
                var versionTyres = _tyresBL.GetTyresByCarVersion(versionId, pageNo, pageSize);

                return Ok(Mapper.Map<VersionTyres, VersionTyresDTO>(versionTyres));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }
    }
}
