using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.Mobile.TrackDay
{
    public class Default : System.Web.UI.Page
    {
        protected bool androidApp;
        protected CMSContent objTrackDayArticles = null;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified By : Sushil Kumar on 26th July 2016
        /// Description : Check for is android app and hide headers and footer 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(Request.QueryString["isapp"]))
                bool.TryParse(Request.QueryString["isapp"], out androidApp);

            LoadTackDayArticlesList();
        }

        /// <summary>
        /// Created By : Sushil Kumar on  15th Dec 2016
        /// Description : Load Track day articles list
        /// </summary>
        private void LoadTackDayArticlesList()
        {
            try
            {
                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.TrackDay);
                string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);

                categorList.Clear();
                categorList = null;

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Bikewale.BAL.EditCMS.Articles>()
                           .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                           .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    objTrackDayArticles = _cache.GetTrackDayArticlesByCategoryList(contentTypeList, 1, 10, 0, 0);
                    if (objTrackDayArticles != null && objTrackDayArticles.Articles != null)
                    {
                        objTrackDayArticles.RecordCount = (uint)objTrackDayArticles.Articles.Count;
                    }

                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, Request.ServerVariables["URL"] + "LoadTackDayArticlesList");
                
            }

        }
    }
}