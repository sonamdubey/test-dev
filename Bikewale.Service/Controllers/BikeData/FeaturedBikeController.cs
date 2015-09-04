using AutoMapper;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.BikeData;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Bikewale.Service.Controllers.BikeData
{
    /// <summary>
    /// Featured Bike Controller
    /// Created By : Sadhana Upadhyay on 21 Aug 2015
    /// </summary>
    public class FeaturedBikeController : ApiController
    {
        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;
        public FeaturedBikeController(IBikeModelsRepository<BikeModelEntity, int> modelRepository)
        {
            _modelRepository = modelRepository;
        }
        #region GetFeaturedBikeList PopulateWhere
        /// <summary>
        /// Created By : Sadhana Upadhyay on 21 aUG 2015
        /// Summary : Get Featured Bike list
        /// </summary>
        /// <param name="topCount">Top Count</param>
        /// <returns></returns>
        public IHttpActionResult Get(uint topCount)
        {
            FeaturedBikeList FeaturedList = new FeaturedBikeList();
            List<FeaturedBikeEntity> objFeature = null;
            try
            {
                objFeature = _modelRepository.GetFeaturedBikes(topCount);                
                FeaturedList.FeaturedBike = FeaturedBikeListMapper.Convert(objFeature);
                if (objFeature != null && objFeature.Count > 0)
                    return Ok(FeaturedList);
                else
                    return NotFound();

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.BikeDiscover.GetFeaturedBikeList");
                objErr.SendMail();
                return InternalServerError();
            }
        }
        #endregion
    }
}