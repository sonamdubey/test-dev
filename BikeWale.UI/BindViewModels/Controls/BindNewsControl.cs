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
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    public class BindNewsControl
    {
        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }

        /// <summary>
        /// Written By : Ashish G. Kamble on 28 Feb 2016
        /// Summary : Function to get the data from the carwale apis. This data is cached on the bikewale.
        /// </summary>
        /// <param name="rptr"></param>
        public void BindNews(Repeater rptr)
        {
            FetchedRecordsCount = 0;

            try
            {
                IEnumerable<ArticleSummary> _objArticleList = null;

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                           .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                           .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    _objArticleList = _cache.GetMostRecentArticlesByIdList(Convert.ToString((int)EnumCMSContentType.News), Convert.ToUInt32(TotalRecords), Convert.ToUInt32(MakeId), Convert.ToUInt32(ModelId));

                    if (_objArticleList != null && _objArticleList.Any())
                    {
                        FetchedRecordsCount = _objArticleList.Count();

                        rptr.DataSource = _objArticleList;
                        rptr.DataBind();
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