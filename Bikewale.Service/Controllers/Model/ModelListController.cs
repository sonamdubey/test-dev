
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Model;
using Bikewale.DTO.Widgets;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.BikeData;
using Bikewale.Service.AutoMappers.Model;
using Bikewale.Service.Utilities;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Model
{
    /// <summary>
    /// To Get List of Models
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// Modified by :   Sumit Kate on 01 Jul 2016
    /// Description :   Added IBikeModelsCacheRepository ref
    /// </summary>
    public class ModelListController : CompressionApiController//ApiController
    {

        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;
        private readonly IBikeModelsCacheRepository<int> _modelCacheRepository = null;
		private readonly IBikeModels<BikeModelEntity, int> _bikeModel;
		private readonly IBikeModelsCacheHelper _bikeModelCacheHelper;

		/// <summary>
		/// Modified by :   Sumit Kate on 01 Jul 2016
		/// Description :   Added IBikeModelsCacheRepository
		/// </summary>
		/// <param name="modelRepository"></param>
		/// <param name="modelCacheRepository"></param>
		/// <param name="bikeModel"></param>
		/// <param name="bikeModelCacheHelper"></param>
		public ModelListController(IBikeModelsRepository<BikeModelEntity, int> modelRepository, IBikeModelsCacheRepository<int> modelCacheRepository, 
			IBikeModels<BikeModelEntity, int> bikeModel, IBikeModelsCacheHelper bikeModelCacheHelper)
        {
            _modelRepository = modelRepository;
            _modelCacheRepository = modelCacheRepository;
            _bikeModel = bikeModel;
			_bikeModelCacheHelper = bikeModelCacheHelper;
		}

        #region Minimum Model Details
        /// <summary>
        /// To Minimum Model Details for Dropdowns 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Minimum Model Details</returns>
        [Obsolete("Unused")]
        [ResponseType(typeof(ModelBase))]
        private IHttpActionResult Get(int modelId)
        {
            BikeModelEntityBase objModel = null;
            ModelBase objDTOModel = null;
            using (IUnityContainer container = new UnityContainer())
                try
                {
                    objModel = _modelRepository.GetById(modelId);

                    if (objModel != null)
                    {
                        objDTOModel = new ModelBase();
                        objDTOModel = ModelMapper.Convert(objModel);
                        return Ok(objDTOModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.ModelListController");
                   
                    return InternalServerError();
                }

            return NotFound();
        }   // Get 
        #endregion

        #region Get Most Popular Bikes
        /// <summary>
        ///  To Get Most popular Bikes List
        ///  Modified by    :   Sumit Kate on 01 Jul 2016
        ///  Description    :   Call the Cache Layer to get the Data
        /// </summary>
        /// <param name="totalCount">No. of records to be fetched</param>
        /// <param name="makeId">Optional (To return Models List Based on MakeID)</param>
        /// <returns>Most Popular bikes based on totalCount and MakeId(Optional)</returns>
        [ResponseType(typeof(MostPopularBikesList))]
        public IHttpActionResult Get(sbyte? totalCount = null, int? makeId = null)
        {
            IEnumerable<MostPopularBikesBase> objModelList = null;
            MostPopularBikesList objDTOModelList = null;
            try
            {
                objModelList = _modelCacheRepository.GetMostPopularBikes(totalCount, makeId);

                if (objModelList != null && objModelList.Any())
                {
                    // Auto map the properties
                    objDTOModelList = new MostPopularBikesList();
                    objDTOModelList.PopularBikes = ModelMapper.Convert(objModelList);

                    return Ok(objDTOModelList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.ModelListController");
               
                return InternalServerError();
            }
            return NotFound();
        }   // Get Most Popular bikes  
        #endregion

        #region Models List
        /// <summary>
        ///  To get list of Models based on request type and makeId  for dropdowns only !
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="makeId"></param>
        /// <returns>Models List for Dropdowns</returns>
        [ResponseType(typeof(ModelList))]
        public IHttpActionResult Get(EnumBikeType requestType, int makeId)
        {
            List<BikeModelEntityBase> objModelList = null;
            ModelList objDTOModelList = null;
            try
            {
                objModelList = _modelRepository.GetModelsByType(requestType, makeId);

                if (objModelList != null && objModelList.Count > 0)
                {
                    objDTOModelList = new ModelList();
                    objDTOModelList.Model = ModelMapper.Convert(objModelList);

                    return Ok(objDTOModelList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.ModelListController");
               
                return InternalServerError();
            }
            return NotFound();
        }   // Get Models list  
        #endregion

        /// <summary>
        /// Created By : Sadhana Upadhyay on 9 Nov 2015
        /// Summary : To get Model by request type
        /// </summary>
        /// <param name="requestType"></param>
        /// <returns></returns>
        [ResponseType(typeof(List<BikeMakeModel>)), Route("api/model/all/{requestType}/")]
        public IHttpActionResult GetAllModels(EnumBikeType requestType)
        {
            IEnumerable<BikeMakeModelEntity> objList = null;
            List<BikeMakeModel> objModelList = null;
            try
            {
                objList = _modelRepository.GetAllModels(requestType);

                if (objList != null && objList.Any())
                {
                    objModelList = new List<BikeMakeModel>();

                    objModelList = MakeModelEntityMapper.Convert(objList.ToList());

                    return Ok(objModelList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.AllModels");
               
                return InternalServerError();
            }
            return NotFound();
        }

		/// <summary>
		/// Created by  : Pratibha Verma on 9 May 2018
		/// Description : returns make with model list
		/// </summary>
		/// <param name="requestType"></param>
		/// <returns></returns>
        [ResponseType(typeof(List<MakeModelList>)), Route("api/pwa/model/all/v2/{requestType}/")]
        public IHttpActionResult GetMakeModelList(EnumBikeType requestType)
        {
            IEnumerable<MakeModelListEntity> objList = null;
            IEnumerable<MakeModelList> objModelList = null;
            try
            {
                objList = _bikeModel.GetMakeModelList(requestType);

                if (objList != null && objList.Any())
                {
                    objModelList = new List<MakeModelList>();

                    objModelList = MakeModelEntityMapper.Convert(objList);

                    return Ok(objModelList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.GetMakeModelList");

                return InternalServerError();
            }
            return NotFound();
        }

    } //class
}  //namespace
