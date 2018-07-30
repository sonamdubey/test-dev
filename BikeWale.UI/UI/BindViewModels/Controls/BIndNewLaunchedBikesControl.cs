using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    public class BindNewLaunchedBikesControl
    {
        public int pageSize { get; set; }
        public int? currentPageNo { get; set; }
        public int FetchedRecordsCount { get; set; }
        public int? makeid { get; set; }

        public void BindNewlyLaunchedBikes(Repeater rptr)
        {
            FetchedRecordsCount = 0;
            IEnumerable<NewLaunchedBikeEntity> objBikeList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<IPager, Pager>()
                        .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    IBikeModelsCacheRepository<int> _objModel = container.Resolve<IBikeModelsCacheRepository<int>>();
                    var _objPager = container.Resolve<IPager>();
                    LaunchedBikeList objLaunched = new LaunchedBikeList();

                    int startIndex = 0, endIndex = 0, curPageNo = 1;

                    curPageNo = currentPageNo.HasValue ? currentPageNo.Value : 1;

                    _objPager.GetStartEndIndex(pageSize, curPageNo, out startIndex, out endIndex);

                    objBikeList = _objModel.GetNewLaunchedBikesList(startIndex, endIndex, makeid).Models;

                    if (objBikeList != null && objBikeList.Any())
                    {
                        FetchedRecordsCount = objBikeList.Count();

                        if (FetchedRecordsCount > 0)
                        {
                            rptr.DataSource = objBikeList;
                            rptr.DataBind();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }
    }
}