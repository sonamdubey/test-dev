using Bikewale.BAL.GrpcFiles;
using Bikewale.Cache.Content;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Content;
using Bikewale.Memcache;
using Grpc.CMS;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Content
{
    public class ViewRT : System.Web.UI.Page
    {
        protected Repeater rptPages, rptPageContent;
        private string _basicId = string.Empty;
        protected ArticlePageDetails objRoadtest;
        protected StringBuilder _bikeTested;
        protected ArticlePhotoGallery ctrPhotoGallery;
        private bool _isContentFount = true;

        static bool _useGrpc = Convert.ToBoolean(ConfigurationManager.AppSettings["UseGrpc"]);
        static readonly ILog _logger = LogManager.GetLogger(typeof(ViewRT));
        static bool _logGrpcErrors = Convert.ToBoolean(ConfigurationManager.AppSettings["LogGrpcErrors"]);

        protected string articleUrl = string.Empty, articleTitle = string.Empty, basicId = string.Empty, authorName = string.Empty, displayDate = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();


            ProcessQS();
            GetRoadtestDetails();
            //GetArticlePhotos();

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IRoadTestCache, RoadTestCache>()
                 .RegisterType<ICacheManager, MemcacheManager>()
                 .RegisterType<IRoadTest, Bikewale.BAL.Content.RoadTest>();
                IRoadTestCache _roadTest = container.Resolve<IRoadTestCache>();

                IEnumerable<ModelImage> objImg = _roadTest.BindPhotos(Convert.ToInt32(_basicId));

                if (objImg != null && objImg.Count() > 0)
                {
                    ctrPhotoGallery.BasicId = Convert.ToInt32(_basicId);
                    ctrPhotoGallery.ModelImageList = objImg;
                    ctrPhotoGallery.BindPhotos();
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        private void ProcessQS()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]) && CommonOpn.CheckId(Request.QueryString["id"]))
            {
                //id = Request.QueryString["id"].ToString();
                _basicId = Request.QueryString["id"];

                string basicId = BasicIdMapping.GetCWBasicId(_basicId);

                Trace.Warn("Carwale basic id : " + basicId);

                if (!String.IsNullOrEmpty(basicId))
                {
                    string _newUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
                    var _titleStartIndex = _newUrl.LastIndexOf('/') + 1;
                    var _titleEndIndex = _newUrl.LastIndexOf('-');
                    string _newUrlTitle = _newUrl.Substring(_titleStartIndex, _titleEndIndex - _titleStartIndex + 1);
                    _newUrl = "/road-tests/" + _newUrlTitle + basicId + ".html";
                    CommonOpn.RedirectPermanent(_newUrl);
                }
            }
            else
            {
                Response.Redirect("/road-tests/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to fetch roadtest details from api asynchronously
        /// </summary>

        private ArticlePageDetails GetFeatureDetailsOldWay(int basicId)
        {
            ArticlePageDetails objFeature=null;
            try
            {

                string _apiUrl = "webapi/article/contentpagedetail/?basicid=" + basicId;
              
                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objFeature = objClient.GetApiResponseSync<ArticlePageDetails>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objFeature);
                }


                if (_logGrpcErrors && objRoadtest != null)
                {
                    _logger.Error(string.Format("Grpc did not work for GetFeatureDetailsOldWay {0}", basicId));
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objFeature;
        }

        public ArticlePageDetails GetFeatureDetails(int basicId)
        {
            ArticlePageDetails objFeature;
            try
            {                
                if (_useGrpc)
                {
                    var _objGrpcFeature = GrpcMethods.GetContentPages((ulong)basicId);

                    if (_objGrpcFeature != null)
                    {
                        objFeature = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcFeature);
                    }
                    else
                    {
                        objFeature = GetFeatureDetailsOldWay(basicId);
                    }
                }
                else
                {
                    objFeature = GetFeatureDetailsOldWay(basicId);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                objFeature = GetFeatureDetailsOldWay(basicId);
            }

            return objFeature;

        }

        private void GetRoadtestDetails()
        {
            try
            {
                objRoadtest = GetFeatureDetails(Convert.ToInt32(_basicId));
                
                if (objRoadtest != null)
                {
                    BindPages();
                    GetRoadtestData();
                    if (objRoadtest.VehiclTagsList.Count > 0)
                        GetTaggedBikeList();
                }
                else
                {
                    _isContentFount = false;
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFount)
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetTaggedBikeList()
        {
            if (objRoadtest.VehiclTagsList.Any(m => (m.MakeBase != null && !String.IsNullOrEmpty(m.MakeBase.MaskingName))))
            {
                _bikeTested = new StringBuilder();

                _bikeTested.Append("Bike Tested: ");

                IEnumerable<int> ids = objRoadtest.VehiclTagsList
                       .Select(e => e.ModelBase.ModelId)
                       .Distinct();

                foreach (var i in ids)
                {
                    VehicleTag item = objRoadtest.VehiclTagsList.Where(e => e.ModelBase.ModelId == i).First();
                    if (!String.IsNullOrEmpty(item.MakeBase.MaskingName))
                    {
                        _bikeTested.Append("<a title='" + item.MakeBase.MakeName + " " + item.ModelBase.ModelName + " Bikes' href='/" + item.MakeBase.MaskingName + "-bikes/" + item.ModelBase.MaskingName + "/'>" + item.ModelBase.ModelName + "</a>   ");
                    }
                }
                Trace.Warn("biketested", _bikeTested.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindPages()
        {
            rptPages.DataSource = objRoadtest.PageList;
            rptPages.DataBind();

            rptPageContent.DataSource = objRoadtest.PageList;
            rptPageContent.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetRoadtestData()
        {
            articleTitle = objRoadtest.Title;
            authorName = objRoadtest.AuthorName;
            displayDate = objRoadtest.DisplayDate.ToString();
            articleUrl = objRoadtest.ArticleUrl;
            basicId = objRoadtest.BasicId.ToString();
        }
    }
}