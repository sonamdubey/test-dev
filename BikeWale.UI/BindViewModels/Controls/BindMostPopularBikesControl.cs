using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    public class BindMostPopularBikesControl
    {
        public int? totalCount { get; set; }
        public int? makeId { get; set; }
        public int FetchedRecordsCount { get; set; }

        /// <summary>
        ///  Modified by    :   Sumit Kate on 01 Jul 2016
        ///  Description    :   Call the Cache Layer to get the Data
        /// </summary>
        /// <param name="rptr"></param>
        public void BindMostPopularBikes(Repeater rptr)
        {
            FetchedRecordsCount = 0;
            IEnumerable<MostPopularBikesBase> popularBase = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();

                    //IBikeModelsRepository<BikeModelEntity, int> objVersion = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();
                    IBikeModelsCacheRepository<int> modelCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                    popularBase = modelCache.GetMostPopularBikes(totalCount, makeId);
                }
                if (popularBase != null)
                {
                    if (popularBase.Count() > 0)
                    {
                        FetchedRecordsCount = popularBase.Count();
                        rptr.DataSource = popularBase;
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
    }
}