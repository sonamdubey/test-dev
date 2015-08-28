﻿using Bikewale.DAL.UsedBikes;
using Bikewale.DTO.UsedBikes;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.UsedBikes;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.UsedBikes
{
    /// <summary>
    /// To Get Popular used Bikes Details
    /// Author : Sushil Kumar
    /// Created On : 28th August 2015
    /// </summary>
    public class PopularUsedBikesController : ApiController
    {
        #region Popular Used Bikes List
        /// <summary>
        /// To get popular used bikes 
        /// </summary>
        /// <param name="topCount">Number of Records to be fetched</param>
        /// <param name="cityId">Optional</param>
        /// <returns></returns>
        [ResponseType(typeof(PopularUsedBikesEntity))]
        public IHttpActionResult Get(uint topCount,int? cityId = null)
        {
            IEnumerable<PopularUsedBikesEntity> objUsedBikesList = null;
            IEnumerable<PopularUsedBikesBase> objDTOUsedBikesList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IUsedBikes usedBikesRepo = null;

                    container.RegisterType<IUsedBikes, UsedBikesRepository>();
                    usedBikesRepo = container.Resolve<IUsedBikes>();

                    objUsedBikesList = usedBikesRepo.GetPopularUsedBikes(topCount, cityId);

                    if (objUsedBikesList != null)
                    {
                        objDTOUsedBikesList = new List<PopularUsedBikesBase>();
                        objDTOUsedBikesList = PopularUsedBikesMapper.Convert(objUsedBikesList);
                        return Ok(objDTOUsedBikesList);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UsedBikes.PopularUsedBikesController");
                objErr.SendMail();
                return InternalServerError();
            }
        }   // Get PopularUsedBikes 
        #endregion

    }
}
