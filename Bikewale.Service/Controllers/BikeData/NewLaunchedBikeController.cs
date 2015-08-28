using Bikewale.BAL.Pager;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.DTO.BikeData;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using AutoMapper;
using Bikewale.DTO.Series;
using Bikewale.Service.AutoMappers.BikeData;

namespace Bikewale.Service.Controllers.BikeData
{
    /// <summary>
    /// New Launched Bike Controller
    /// Created By : Sadhana Upadhyay on 25 Aug 2015
    /// </summary>
    public class NewLaunchedBikeController : ApiController
    {
        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;
        private readonly IPager _objPager = null;
        public NewLaunchedBikeController(IBikeModelsRepository<BikeModelEntity, int> modelRepository, IPager objPager)
        {
            _modelRepository = modelRepository;
            _objPager = objPager;
        }
        /// <summary>
        /// Created By : Sadhana Upadhyay on 25 Aug 2015
        /// Summary : To get Recently Launched Bike
        /// </summary>
        /// <param name="pageSize">No. of Record</param>
        /// <param name="curPageNo">Current Page No. (Optional)</param>
        /// <returns></returns>
        public IHttpActionResult Get(int pageSize, int? curPageNo = null)
        {
            int recordCount = 0;
            try
            {
                LaunchedBikeList objLaunched = new LaunchedBikeList();
                int startIndex = 0, endIndex = 0, currentPageNo = 1;
                currentPageNo = curPageNo.HasValue ? curPageNo.Value : 1;

                _objPager.GetStartEndIndex(pageSize, currentPageNo, out startIndex, out endIndex);

                List<NewLaunchedBikeEntity> objRecent = _modelRepository.GetNewLaunchedBikesList(startIndex, endIndex, out recordCount);
                                
                objLaunched.LaunchedBike = LaunchedBikeListMapper.Convert(objRecent);

                if (objRecent != null && objRecent.Count > 0)
                    return Ok(objLaunched);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.BikeDiscover.GetRecentlyLaunchedBikeList");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}