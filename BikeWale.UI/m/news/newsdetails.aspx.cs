﻿using Bikewale.BAL.CMS;
using Bikewale.BAL.GrpcFiles;
using Bikewale.Common;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.CMS;
using Bikewale.Memcache;
using Grpc.CMS;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 21 May 2014
    /// Modified By : Ashwini Todkar on  on 30 Sept 2014
    /// </summary>
    public class newsdetails : System.Web.UI.Page
    {
        //private CMSPageDetailsEntity pageDetails = null;
        protected String nextPageUrl = String.Empty, prevPageUrl = String.Empty, prevPageTitle = String.Empty, nextPageTitle = String.Empty, pageUrl = String.Empty;
        protected String newsContent = String.Empty, newsTitle = String.Empty, author = String.Empty, displayDate = String.Empty, mainImg = String.Empty;
        protected uint pageViews = 0;
        protected String _newsId = String.Empty;
        private ArticleDetails objNews = null;
        private bool _isContentFound = true;

        static bool _useGrpc = Convert.ToBoolean(ConfigurationManager.AppSettings["UseGrpc"]);
        static bool _logGrpcErrors = Convert.ToBoolean(ConfigurationManager.AppSettings["LogGrpcErrors"]);
        static readonly ILog _logger = LogManager.GetLogger(typeof(newsdetails));

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProcessQS();

                int _basicId = 0;

                if (Int32.TryParse(_newsId, out _basicId))
                    GetNewsDetails(_basicId);
                else
                {
                    Response.Redirect("/m/pagenotfound.aspx", true);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// PopulateWhere to process query string and get carwale new basicid against bikewale basicid
        /// </summary>
        private void ProcessQS()
        {
            if (Request.QueryString["id"] != null && !String.IsNullOrEmpty(Request.QueryString["id"]))
            {

                /** Modified By : Ashwini Todkar on 19 Aug 2014 , add when consuming carwale api
               //Check if basic id exists in mapped carwale basic id log **/
                string basicid = BasicIdMapping.GetCWBasicId(Request["id"]);
                Trace.Warn("basicid" + basicid);
                //if id exists then redirect url to new basic id url
                if (!String.IsNullOrEmpty(basicid))
                {
                    string newUrl = "/m/news/" + basicid + "-" + Request["t"] + ".html";
                    Trace.Warn("New url : " + newUrl);
                    CommonOpn.RedirectPermanent(newUrl);
                }
                else
                {
                    if (CommonOpn.CheckId(Request.QueryString["id"]) == true)
                        _newsId = Request.QueryString["id"];
                }
            }
            else
            {
                Response.Redirect("/m/news/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        /// <summary>
        ///  PopulateWhere to set news details from carwale api asynchronously
        /// </summary>
        /// <param name="_basicId"></param>
        private void GetNewsDetails(int _basicId)
        {
            try
            {
                GetNewsDetailsViaGrpc(_basicId);               

                if (objNews != null)
                    GetNewsData();
                else
                    _isContentFound = false;
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFound)
                {
                    Response.Redirect("/m/news/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        private void GetNewsDetailsViaGrpc(int _basicId)
        {
            try
            {
                if (_useGrpc)
                {
                    var _objGrpcArticle = GrpcMethods.GetContentDetails(Convert.ToUInt64(_basicId));

                    if (_objGrpcArticle != null)
                    {
                        objNews = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticle);
                    }
                    else
                    {
                        GetNewsDetailsOldWay(_basicId);
                    }
                }
                else
                {
                    GetNewsDetailsOldWay(_basicId);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                GetNewsDetailsOldWay(_basicId);
            }
        }

        private async void GetNewsDetailsOldWay(int _basicId)
        {
            try
            {
                if (_logGrpcErrors)
                {
                    _logger.Error(string.Format("Grpc did not work for GetArticlePhotos {0}", _basicId));
                }
                string _apiUrl = "webapi/article/contentdetail/?basicid=" + _basicId;

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objNews = await objClient.GetApiResponse<ArticleDetails>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objNews);
                }

                if (objNews != null)
                    GetNewsData();
                else
                    _isContentFound = false;
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFound)
                {
                    Response.Redirect("/m/news/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// PopulateWhere to set news details
        /// </summary>
        private void GetNewsData()
        {
            newsTitle = objNews.Title;
            author = objNews.AuthorName;
            displayDate = objNews.DisplayDate.ToString();
            newsContent = objNews.Content;
            nextPageTitle = objNews.NextArticle.Title;
            prevPageTitle = objNews.PrevArticle.Title;
            pageViews = objNews.Views;
            pageUrl = _newsId + '-' + objNews.ArticleUrl + ".html";

            if (!String.IsNullOrEmpty(objNews.NextArticle.ArticleUrl ))
                nextPageUrl = objNews.NextArticle.BasicId + "-" + objNews.NextArticle.ArticleUrl + ".html";
            
            if (!String.IsNullOrEmpty(objNews.PrevArticle.ArticleUrl))
                prevPageUrl = objNews.PrevArticle.BasicId + "-" + objNews.PrevArticle.ArticleUrl + ".html";            
        }

        protected String GetMainImagePath()
        {
            String mainImgUrl = String.Empty;
            //mainImgUrl = ImagingFunctions.GetPathToShowImages( objNews.LargePicUrl, objNews.HostUrl);
            mainImgUrl = Bikewale.Utility.Image.GetPathToShowImages(objNews.OriginalImgUrl, objNews.HostUrl, Bikewale.Utility.ImageSize._640x348);

            return mainImgUrl;
        }
    }
}