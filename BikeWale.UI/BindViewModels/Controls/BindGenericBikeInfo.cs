﻿using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Cache.GenericBikes;
using Bikewale.DAL.GenericBikes;
using Bikewale.DAL.NewBikeSearch;
using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.GenericBikes;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By : Sushil Kumar on 2nd Jan 2016
    /// Generic Bike Functions
    /// </summary>
    public class BindGenericBikeInfo
    {
        public uint ModelId { get; set; }

        /// <summary>
        /// Created By : Sushil Kumar on 2nd Jan 2016
        /// Summary :  To get generic bike info by modelid
        /// </summary>
        public GenericBikeInfo GetGenericBikeInfo()
        {
            GenericBikeInfo genericBikeInfo = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBestBikesCacheRepository, BestBikesCacheRepository>()
                            .RegisterType<ISearchResult, SearchResult>()
                            .RegisterType<ICacheManager, MemcacheManager>()
                            .RegisterType<IArticles, Articles>()
                            .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                            .RegisterType<IProcessFilter, ProcessFilter>()
                            .RegisterType<IGenericBikeRepository, GenericBikeRepository>()
                           .RegisterType<ICacheManager, MemcacheManager>();
                    var _objGenericBike = container.Resolve<IBestBikesCacheRepository>();

                    genericBikeInfo = _objGenericBike.GetGenericBikeInfo(ModelId);
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BindGenericBikeInfo.GetGenericBikeInfo");
            }
            return genericBikeInfo;
        }
    }
}