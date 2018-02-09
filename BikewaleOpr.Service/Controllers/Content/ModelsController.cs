using Bikewale.Notifications;
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Service.AutoMappers.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers.Content
{
    public class ModelsController : ApiController
    {
        private readonly IBikeModelsRepository _modelsRepo = null;

        public ModelsController(IBikeModelsRepository modelsRepo)
        {
            _modelsRepo = modelsRepo;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 18n Apr 2017
        /// Summary : API to get the bikemodels list for the given make id and request type
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="requestType">
        /// 1 PRICEQUOTE
        /// 2 NEW
        /// 3 USED
        /// 4 UPCOMING
        /// 5 ROADTEST
        /// 6 COMPARISONTEST
        /// 7 ALL
        /// 8 user reviews
        /// </param>
        /// <returns>API return list of bikemodels</returns>
        [HttpGet, Route("api/models/makeid/{makeId}/requestType/{requestType}")]
        public IHttpActionResult GetModels(uint makeId, ushort requestType)
        {
            try
            {
                if (makeId > 0 && requestType > 0)
                {
                    IEnumerable<BikeModelEntityBase> objModels = _modelsRepo.GetModels(makeId, requestType);

                    if (objModels != null && objModels.Any())
                    {
                        IEnumerable<ModelBase> objModelsDTO = BikeModelsMapper.Convert(objModels);

                        return Ok(objModelsDTO);
                    }
                    else {
                        return Ok();
                    }                    
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Service.Controllers.Content.GetModels");
                return InternalServerError();
            }
        }   // End of GetModels

        /// <summary>
        /// Created by: Vivek Singh Tomar on 7th Aug 2017
        /// Summary: Api to versions for given model id.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [HttpGet, Route("api/models/{modelid}/versions/{requesttype}/")]
        public IHttpActionResult GetVersions(EnumBikeType requestType, uint modelId)
        {
            IEnumerable<VersionBase> objBikeVersionBaseList = null;
            if(modelId > 0)
            {
                try
                {
                    IEnumerable<BikeVersionEntityBase> objBikeVersionEntityBaseList = _modelsRepo.GetVersionsByModel(requestType, modelId);
                    objBikeVersionBaseList = BikeVersionsMapper.Convert(objBikeVersionEntityBaseList);
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "BikewaleOpr.Service.Controllers.Content.ModelsController.GetVersions");
                    return InternalServerError();
                }
            }
            else
            {
                return BadRequest("Invalid Model Id");
            }
            return Ok(objBikeVersionBaseList);
        }

		/// <summary>
		/// Written By : Ashish G. Kamble on 2 Feb 2018
		/// Summary : API to save the model page views from google analytics
		/// </summary>
		/// <param name="modelsList">Send data from body. Value should be ModelId1:Views1, ModelId2:Views2 (22:352,686:1265) only</param>
		/// <returns>Returns Ok if data inserted successfully</returns>
		[HttpPost, Route("api/models/pageviews/")]
		public IHttpActionResult SavePageViews([FromBody]string modelsList)
		{
			try
			{
				if (String.IsNullOrEmpty(modelsList))
				{
					return BadRequest("modelsList is null or empty");
				}
				else
				{
					bool isSuccess = _modelsRepo.SavePageViews(modelsList);

					if (isSuccess)
						return Ok("Data Inserted Successfully");
					else
						return InternalServerError();
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "BikewaleOpr.Service.Controllers.Content.SavePageViews");
				return InternalServerError();
			}
		}

	}   // class
}   // namespace
