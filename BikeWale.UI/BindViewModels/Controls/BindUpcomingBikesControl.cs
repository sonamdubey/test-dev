using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Modified By :  Aditi Srivastava on 10 Nov 2016
    /// Description:  Added function to fetch upcoming bikes
    /// </summary>
    /// <returns></returns>
    public class BindUpcomingBikesControl
    {

        public int sortBy { get; set; }
        public int pageSize { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int? curPageNo { get; set; }
        public int FetchedRecordsCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rptr"></param>
        public void BindUpcomingBikes(Repeater rptr)
        {
            FetchedRecordsCount = 0;

            IEnumerable<UpcomingBikeEntity> objBikeList = null;

            try
            {

                EnumUpcomingBikesFilter filter = EnumUpcomingBikesFilter.Default;

                switch (sortBy)
                {
                    case 0: filter = EnumUpcomingBikesFilter.Default;
                        break;
                    case 1: filter = EnumUpcomingBikesFilter.PriceLowToHigh;
                        break;
                    case 2: filter = EnumUpcomingBikesFilter.PriceHighToLow;
                        break;
                    case 3: filter = EnumUpcomingBikesFilter.LaunchDateSooner;
                        break;
                    case 4: filter = EnumUpcomingBikesFilter.LaunchDateLater;
                        break;
                }

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>();

                    var objCache = container.Resolve<IBikeModelsCacheRepository<int>>();

                    objBikeList = objCache.GetUpcomingBikesList(filter, pageSize, MakeId, ModelId, curPageNo);

                }


                if (objBikeList != null)
                {
                    FetchedRecordsCount = objBikeList.Count();

                    if (FetchedRecordsCount > 0)
                    {
                        rptr.DataSource = objBikeList;
                        rptr.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        /// <summary>
        /// Created By :  Aditi Srivastava on 10 Nov 2016
        /// Description:  Get upcoming bikes list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UpcomingBikeEntity> GetUpcomingBikes()
        {
            FetchedRecordsCount = 0;

            IEnumerable<UpcomingBikeEntity> objBikeList = null;

            try
            {

                EnumUpcomingBikesFilter filter = EnumUpcomingBikesFilter.Default;

                switch (sortBy)
                {
                    case 0: filter = EnumUpcomingBikesFilter.Default;
                        break;
                    case 1: filter = EnumUpcomingBikesFilter.PriceLowToHigh;
                        break;
                    case 2: filter = EnumUpcomingBikesFilter.PriceHighToLow;
                        break;
                    case 3: filter = EnumUpcomingBikesFilter.LaunchDateSooner;
                        break;
                    case 4: filter = EnumUpcomingBikesFilter.LaunchDateLater;
                        break;
                }

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>();

                    var objCache = container.Resolve<IBikeModelsCacheRepository<int>>();

                    objBikeList = objCache.GetUpcomingBikesList(filter, pageSize, MakeId, ModelId, curPageNo);
                    if (objBikeList != null)
                        FetchedRecordsCount = objBikeList.Count();
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objBikeList;

        }
    }


}