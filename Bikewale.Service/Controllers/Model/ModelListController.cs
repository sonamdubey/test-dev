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
using AutoMapper;
using System.Web.Http.Description;
using Bikewale.DTO.Model;
using Bikewale.Service.AutoMappers.Model;
using Bikewale.DTO.Widgets;
using Bikewale.DTO.Version;
using Bikewale.DTO.Make;
using Bikewale.Notifications;

namespace Bikewale.Service.Controllers.Model
{
    /// <summary>
    /// To Get List of Models
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class ModelListController : ApiController
    {

        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;
        public ModelListController(IBikeModelsRepository<BikeModelEntity, int> modelRepository)
        {
            _modelRepository = modelRepository;
        }
    
        #region Minimum Model Details
        /// <summary>
        /// To Minimum Model Details for Dropdowns 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Minimum Model Details</returns>
        [ResponseType(typeof(ModelBase))]
        public IHttpActionResult Get(int modelId)
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
                    ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Model.ModelListController");
                    objErr.SendMail();
                    return InternalServerError();
                }

            return NotFound();
        }   // Get 
        #endregion     

        #region Get Most Popular Bikes
        /// <summary>
        ///  To Get Most popular Bikes List
        /// </summary>
        /// <param name="totalCount">No. of records to be fetched</param>
        /// <param name="makeId">Optional (To return Models List Based on MakeID)</param>
        /// <returns>Most Popular bikes based on totalCount and MakeId(Optional)</returns>
        [ResponseType(typeof(IEnumerable<MostPopularBikesList>))]
        public IHttpActionResult Get(sbyte totalCount, int? makeId = null)
        {
            List<MostPopularBikesBase> objModelList = null;
            MostPopularBikesList objDTOModelList = null;
            try
            {
                objModelList = _modelRepository.GetMostPopularBikes(totalCount, makeId);

                if (objModelList != null && objModelList.Count > 0)
                {
                    // Auto map the properties
                    objDTOModelList = new MostPopularBikesList();
                    Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                    Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                    Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
                    Mapper.CreateMap<MostPopularBikesBase, MostPopularBikes>();
                    objDTOModelList.PopularBikes = Mapper.Map<List<MostPopularBikesBase>, List<MostPopularBikes>>(objModelList);

                    return Ok(objDTOModelList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Model.ModelListController");
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Model.ModelListController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }   // Get Models list  
        #endregion


    } //class
}  //namespace
