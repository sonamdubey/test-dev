﻿using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Memcache;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Mobile.TrackDay
{
    public class Details : System.Web.UI.Page
    {
        protected bool androidApp;
        protected String _trackdayId = String.Empty;
        protected ArticleDetails objTrackDay = null;
        protected CMSContent objTrackDayArticles = null;
        protected IEnumerable<ModelImage> objImages = null;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            ProcessQS();

            int _basicId = 0;

            if (Int32.TryParse(_trackdayId, out _basicId))
            {
                GetTrackDayDetails(_basicId);

            }
            else
            {
                Response.Redirect("/m/pagenotfound.aspx", true);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

        }

        private void GetRelatedArticles()
        {
            throw new NotImplementedException();
        }

        private void GetTrackDayDetails(int _basicId)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Bikewale.BAL.EditCMS.Articles>()
                            .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                            .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    objTrackDay = _cache.GetNewsDetails(Convert.ToUInt32(_basicId));
                    objTrackDayArticles = _cache.GetArticlesByCategoryList(((int)EnumCMSContentType.TrackDay).ToString(), 1, 10, 0, 0);
                    objImages = _cache.GetArticlePhotos(Convert.ToInt32(_basicId));

                    if (objTrackDayArticles != null && objTrackDayArticles.Articles != null)
                    {
                        objTrackDayArticles.Articles = objTrackDayArticles.Articles.Where(x => x.BasicId != Convert.ToUInt32(_basicId)).ToList();
                        objTrackDayArticles.RecordCount = (uint)objTrackDayArticles.Articles.Count;
                    }


                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, string.Format("Url : {0}-GetTrackDayDetails , BasicId : {1}", Request.ServerVariables["URL"], _basicId));
                objErr.SendMail();
            }
            finally
            {
                if (objTrackDayArticles != null && objTrackDayArticles.RecordCount < 1)
                {
                    Response.Redirect("/m/trackday2016/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// Populate Where to process query string and get carwale new basicid against bikewale basicid
        /// </summary>
        private void ProcessQS()
        {
            if (Request.QueryString["id"] != null && !String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                if (!String.IsNullOrEmpty(Request.QueryString["isapp"]))
                    bool.TryParse(Request.QueryString["isapp"], out androidApp);

                //Check if basic id exists in mapped carwale basic id log **/
                string basicid = BasicIdMapping.GetCWBasicId(Request["id"]);
                //if id exists then redirect url to new basic id url
                if (!basicid.Equals(Request.QueryString["id"]))
                {
                    string newUrl = string.Format("/m/trackday2016/{0}-{1}.html", basicid, Request["t"]);
                    CommonOpn.RedirectPermanent(newUrl);
                }
                else
                {
                    if (CommonOpn.CheckId(Request.QueryString["id"]))
                        _trackdayId = Request.QueryString["id"];
                }
            }
            else
            {
                Response.Redirect("/m/trackday2016/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }
    }
}