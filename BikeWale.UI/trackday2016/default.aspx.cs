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

namespace Bikewale.TrackDay
{
    public class Default : System.Web.UI.Page
    {
        protected CMSContent objTrackDayArticles = null;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016

            Form.Action = Request.RawUrl;

            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            Bikewale.Common.DeviceDetection dd = new Bikewale.Common.DeviceDetection(originalUrl);
            dd.DetectDevice();

            LoadTackDayArticlesList();
        }

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