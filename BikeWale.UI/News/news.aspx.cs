using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Memcache;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web;


namespace Bikewale.News
{
    /// <summary>
    /// Created By : Ashwini Todkar on 3 Oct 2014
    /// </summary>
    public class news : System.Web.UI.Page
    {
        private string _basicId = string.Empty;
        bool _isContentFount = true;
        protected ArticleDetails objArticle = null;

        protected string articleUrl = string.Empty, articleTitle = string.Empty, HostUrl = string.Empty, basicId = string.Empty, smallPicUrl = string.Empty, authorName = string.Empty, nextPageArticle = string.Empty, prevPageArticle = string.Empty, originalImgUrl = string.Empty;
        protected string displayDate = string.Empty, mainImgCaption = string.Empty, largePicUrl = string.Empty, content = string.Empty, prevPageUrl = string.Empty, nextPageUrl = string.Empty, hostUrl = string.Empty;
        protected bool isMainImageSet = false;
        protected MostPopularBikesMin ctrlPopularBikes;
        private BikeMakeEntityBase _taggedMakeObj;

        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }
        private void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            //code for device detection added by Ashwini Todkar
            // Modified By :Ashish Kamble on 5 Feb 2016

            Form.Action = Request.RawUrl;

            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();

            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();


            ProcessQS();
            if (!String.IsNullOrEmpty(_basicId))
                GetNewsArticleDetails();

            ctrlPopularBikes.totalCount = 4;
            ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
            ctrlPopularBikes.cityName = currentCityArea.City;
            if (_taggedMakeObj != null)
            {
                ctrlPopularBikes.makeId = _taggedMakeObj.MakeId;
                ctrlPopularBikes.makeName = _taggedMakeObj.MakeName;
                ctrlPopularBikes.makeMasking = _taggedMakeObj.MaskingName;
            }

        }

        private void ProcessQS()
        {
            if (Request["id"] != null && Request.QueryString["id"] != string.Empty)
            {
                /** Modified By : Ashwini Todkar on 12 Aug 2014 , add when consuming carwale api
                //Check if basic id exists in mapped carwale basic id log **/
                _basicId = BasicIdMapping.GetCWBasicId(Request["id"]);

                //if id exists then redirect url to new basic id url
                if (!String.IsNullOrEmpty(_basicId))
                {
                    string newUrl = "/news/" + _basicId + "-" + Request["t"] + ".html";
                    CommonOpn.RedirectPermanent(newUrl);    //302 redirection to new basic id
                }
                else
                {
                    _basicId = Request["id"];
                }
            }
            else
            {
                Response.Redirect("/pagenotfound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 24 Sept 2014
        /// PopulateWhere to fetch news details from api asynchronously
        /// </summary>
        private void GetNewsArticleDetails()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                            .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                            .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    objArticle = _cache.GetNewsDetails(Convert.ToUInt32(_basicId));

                    if (objArticle == null)
                        _isContentFount = false;
                    else
                    {
                        GetNewsData();
                        GetTaggedBikeList();
                    }

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
                    Response.Redirect("/news/", false);
                    if (HttpContext.Current != null)
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }




        //PopulateWhere to set news details
        private void GetNewsData()
        {
            articleTitle = objArticle.Title;
            authorName = objArticle.AuthorName;
            displayDate = objArticle.DisplayDate.ToString();
            articleUrl = objArticle.ArticleUrl;
            HostUrl = objArticle.HostUrl;
            basicId = objArticle.BasicId.ToString();
            smallPicUrl = objArticle.SmallPicUrl;
            mainImgCaption = objArticle.MainImgCaption;
            largePicUrl = objArticle.LargePicUrl;
            content = objArticle.Content;
            prevPageUrl = "/news/" + objArticle.PrevArticle.BasicId + "-" + objArticle.PrevArticle.ArticleUrl + ".html";
            nextPageUrl = "/news/" + objArticle.NextArticle.BasicId + "-" + objArticle.NextArticle.ArticleUrl + ".html";
            isMainImageSet = objArticle.IsMainImageSet;
            originalImgUrl = objArticle.OriginalImgUrl;
        }

        /// <summary>
        /// Written By : Ashwini Todkar to get main image
        /// </summary>
        /// <returns></returns>
        protected String GetMainImagePath()
        {
            String mainImgUrl = String.Empty;
            //mainImgUrl = ImagingFunctions.GetPathToShowImages(objArticle.LargePicUrl, objArticle.HostUrl);
            mainImgUrl = ImagingFunctions.GetPathToShowImages(objArticle.OriginalImgUrl, objArticle.HostUrl, Bikewale.Utility.ImageSize._640x348);

            return mainImgUrl;
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetTaggedBikeList()
        {
            if (objArticle != null && objArticle.VehiclTagsList != null)
            {

                var taggedMakeObj = objArticle.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.MakeBase.MaskingName));
                if (taggedMakeObj != null)
                {
                    _taggedMakeObj = taggedMakeObj.MakeBase;
                }
                else
                {
                    _taggedMakeObj = objArticle.VehiclTagsList.FirstOrDefault().MakeBase;
                    FetchMakeDetails();
                }
            }
        }

        private void FetchMakeDetails()
        {

            if (_taggedMakeObj != null && _taggedMakeObj.MakeId > 0)
            {

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                    var makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
                    _taggedMakeObj = makesRepository.GetMakeDetails(_taggedMakeObj.MakeId.ToString());

                }
            }
        }


    }
}