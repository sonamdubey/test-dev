using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Modified By : Sushil Kumar on 10th Nov 2016
    /// Description : Added provision to bind most popular bikes for edit cms
    /// </summary>
    public class BindMostPopularBikesControl
    {
        public int? totalCount { get; set; }
        public int? makeId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public int? cityId { get; set; }
        public IEnumerable<MostPopularBikesBase> popularBikes = null;
        private const ushort TotalWidgetItems = 9;

        /// <summary>
        ///  Modified by :   Sumit Kate on 01 Jul 2016
        ///  Description :   Call the Cache Layer to get the Data
        /// Modified By : Sushil Kumar on 10th Nov 2016
        /// Description : Added provision to bind most popular bikes for edit cms
        /// </summary>
        /// <param name="rptr"></param>
        public void BindMostPopularBikes(Repeater rptr)
        {
            FetchedRecordsCount = 0;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<IPager, Pager>()
                        .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();

                    IBikeModelsCacheRepository<int> modelCache = container.Resolve<IBikeModelsCacheRepository<int>>();

                    popularBikes = modelCache.GetMostPopularBikes(TotalWidgetItems, makeId);
                }
                if (popularBikes != null && popularBikes.Any())
                {
                    popularBikes = popularBikes.Take(Convert.ToInt32(totalCount));

                    if (rptr != null)
                    {
                        rptr.DataSource = popularBikes;
                        rptr.DataBind();
                    }
                    FetchedRecordsCount = popularBikes.Count();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
        }
        /// <summary>
        /// created by Subodh Jain on 22 sep 2016
        /// des :- to fetch details for popular bikes widget 
        /// Modified By : Sushil Kumar on 10th Nov 2016
        /// Description : Added provision to bind most popular bikes for edit cms
        /// Modified By : Sushil Kumar on 16th Nov 2016
        /// Description : Handle top count for other pages edit cms
        /// </summary>
        /// <param name="rptr"></param>
        public void BindMostPopularBikesMakeCity(Repeater rptr)
        {
            FetchedRecordsCount = 0;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<IPager, Pager>()
                        .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();


                    IBikeModelsCacheRepository<int> modelCache = container.Resolve<IBikeModelsCacheRepository<int>>();

                    popularBikes = modelCache.GetMostPopularBikesbyMakeCity(TotalWidgetItems, Convert.ToUInt32(makeId), Convert.ToUInt32(cityId));
                }
                if (popularBikes != null && popularBikes.Any())
                {
                    popularBikes = popularBikes.Take(Convert.ToInt32(totalCount));

                    if (rptr != null)
                    {
                        rptr.DataSource = popularBikes;
                        rptr.DataBind();
                    }
                    FetchedRecordsCount = popularBikes.Count();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BindMostPopularBikesControl.BindMostPopularBikesMakeCity");

            }
        }
    }
}