using Bikewale.Common;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.BikeData;
using Bikewale.BAL.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Cache.Core;
using Bikewale.Cache.BikeData;

namespace Bikewale.BindViewModels.Controls
{
    public class BindUpcomingBikesControl
    {

        public int sortBy { get; set; }
        public int pageSize { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int? curPageNo { get; set; }
        public int FetchedRecordsCount { get; set; }

        public void BindUpcomingBikes(Repeater rptr)
        {
            FetchedRecordsCount = 0;

            //UpcomingBikeList objBikeList = null;
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
                             .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>();

                    var objCache = container.Resolve<IBikeModelsCacheRepository<int>>();

                    objBikeList = objCache.GetUpcomingBikesList(filter, pageSize, MakeId, ModelId, curPageNo);

                }

                //string _apiUrl = String.Format("/api/UpcomingBike/?sortBy={0}&pageSize={1}", sortBy, pageSize);
                
                //if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                //{
                //    _apiUrl += String.Format("&makeId={0}&curPageNo={1}", MakeId, curPageNo);

                //    if (ModelId.HasValue && ModelId.Value > 0)
                //    {
                //        _apiUrl += String.Format("&modelId={0}", ModelId);
                //    }
                //}

                //objBikeList = BWHttpClient.GetApiResponseSync<UpcomingBikeList>(_bwHostUrl, _requestType, _apiUrl, objBikeList);

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

    }
}