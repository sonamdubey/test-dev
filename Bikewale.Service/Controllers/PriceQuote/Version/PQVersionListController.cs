using Bikewale.DTO.PriceQuote.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote.Version;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.Version
{
    /// <summary>
    /// Price Quote Version List controller
    /// Author  :   Sumit Kate
    /// Created On  :   20 Aug 2015
    /// </summary>
    public class PQVersionListController : ApiController
    {
        private readonly IBikeVersionCacheRepository<BikeVersionEntity,uint> _objVersion = null;
        private readonly IPriceQuoteCache _priceQuoteCache;

        /// <summary>
        /// 
        /// </summary>
        public PQVersionListController(IBikeVersionCacheRepository<BikeVersionEntity,uint> objVersion, IPriceQuoteCache priceQuoteCache)
        {
            _objVersion = objVersion;
            _priceQuoteCache = priceQuoteCache;
        }
        /// <summary>
        /// Gets the Version list for given model and city
        /// </summary>
        /// <param name="modelId">model Id</param>
        /// <param name="cityId">city id</param>
        /// <returns></returns>
        [ResponseType(typeof(PQVersionList))]
        public IHttpActionResult Get(uint modelId, int? cityId = null)
        {
			try
			{
				PQVersionList objDTOVersionList = GetVersionList(modelId, cityId);
				if (objDTOVersionList != null)
				{
					return Ok(objDTOVersionList);
				}
				else
				{
					return NotFound();
				}
			}
			catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Version.PQVersionListController.Get");
               
                return InternalServerError();
            }
        }

		/// <summary>
		/// Created by  : Pratibha Verma on 11 May 2018
		/// Description : finance PQVersion List
		/// </summary>
		/// <param name="modelId"></param>
		/// <param name="cityId"></param>
		/// <returns></returns>
		[ResponseType(typeof(PQVersionList)), Route("api/pwa/PQVersionList/")]
		public IHttpActionResult GetPQVersionList(uint modelId, uint cityId )
		{
			try
			{
                if (modelId > 0 && cityId > 0)
                {

                    IEnumerable<Entities.PriceQuote.OtherVersionInfoEntity> otherVersionInfoEntity = _priceQuoteCache.GetOtherVersionsPrices(modelId, cityId);
                    PQVersionList objDTOVersionList = new PQVersionList();
                    objDTOVersionList.Versions = PQVersionListMapper.Convert(otherVersionInfoEntity);

                    if (objDTOVersionList != null)
                    {
                        return Ok(objDTOVersionList);
                    }
                    else
                    {
                        return NotFound();
                    } 
                }
                else
                {
                    return BadRequest();
                }
            }
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Version.PQVersionListController.Get");
				return InternalServerError();
			}
		}

		private PQVersionList GetVersionList(uint modelId, int? cityId = null)
		{
			PQVersionList objDTOVersionList = null;
			try
			{
				IEnumerable<BikeVersionsListEntity> objVersionList = _objVersion.GetVersionsByType(EnumBikeType.PriceQuote, Convert.ToInt32(modelId), cityId);
				if (objVersionList != null)
				{
					objDTOVersionList = new PQVersionList();
					objDTOVersionList.Versions = PQVersionListMapper.Convert(objVersionList);
				}

			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Version.PQVersionListController.GetVersionList()");
			}
			return objDTOVersionList;
		}
	}
}
