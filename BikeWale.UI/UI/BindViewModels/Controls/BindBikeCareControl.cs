using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By :Subodh Jain on 8 nov 2016
    /// Summary : Class have functions to bind the MaintainanceTips reviews.
    /// </summary>
    public class BindBikeCareControl
    {
        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }
        /// <summary>
        /// Created By :Subodh Jain on 8 nov 2016
        /// Summary :  function to bind the MaintainanceTips reviews.
        /// </summary>
        public IEnumerable<ArticleSummary> MaintainanceTips()
        {

            IEnumerable<ArticleSummary> objArticleList = null;

            try
            {
                string _contentType = Convert.ToString((int)EnumCMSContentType.TipsAndAdvices);



                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                           .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                           .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent objArticles = container.Resolve<ICMSCacheContent>();

                    objArticleList = objArticles.GetMostRecentArticlesByIdList(_contentType, Convert.ToUInt32(TotalRecords), Convert.ToUInt32(MakeId), Convert.ToUInt32(ModelId));
                }



            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BindMaintainanceTipsControl.MaintainanceTips");
                
            }
            return objArticleList;
        }
    }
}