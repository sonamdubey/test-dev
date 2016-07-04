﻿using Bikewale.BAL.BikeData;
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
    public class BindMakePage
    {
        public int totalCount { get; set; }
        public int makeId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeDescriptionEntity BikeDesc { get; set; }
        public Int64 MinPrice { get; set; }
        public Int64 MaxPrice { get; set; }

        public void BindMostPopularBikes(Repeater rptr)
        {
            FetchedRecordsCount = 0;

            BikeDescriptionEntity description = null;
            IEnumerable<MostPopularBikesBase> objModelList = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {

                      //  .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();

                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                            .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                            .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                            .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                            .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                            .RegisterType<ICacheManager, MemcacheManager>()
                            
                            ;
                    var _objMakeCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                    var _objModelCache = container.Resolve<IBikeModelsCacheRepository<int>>();

                    description = _objMakeCache.GetMakeDescription(makeId);
                    Make = _objMakeCache.GetMakeDetails(Convert.ToUInt32(makeId));
                    objModelList = _objModelCache.GetMostPopularBikesByMake(makeId);

                }

                if (objModelList != null && objModelList.Count() > 0)
                {
                    FetchedRecordsCount = objModelList.Count();
                    Make = objModelList.FirstOrDefault().objMake;
                    BikeDesc = description;
                    MinPrice = objModelList.Min(bike => bike.VersionPrice);
                    MaxPrice = objModelList.Max(bike => bike.VersionPrice);

                    rptr.DataSource = objModelList;
                    rptr.DataBind();
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