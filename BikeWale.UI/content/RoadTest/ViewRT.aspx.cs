﻿using Bikewale.BAL.EditCMS;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Memcache;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Content
{
    /// <summary>
    /// Modified By : Aditi Srivastava on 10 Nov 2016
    /// Description : Added control for upcoming bikes widget
    /// </summary>
    public class ViewRT : System.Web.UI.Page
    {
        protected UpcomingBikesMinNew ctrlUpcomingBikes;
        protected Repeater rptPages, rptPageContent;
        private string _basicId = string.Empty;
        protected ArticlePageDetails objRoadtest;
        protected StringBuilder _bikeTested;
        protected ArticlePhotoGallery ctrPhotoGallery;
        protected ModelGallery ctrlModelGallery;
        protected MostPopularBikesMin ctrlPopularBikes;
        private GlobalCityAreaEntity currentCityArea;
        private BikeMakeEntityBase _taggedMakeObj;
        private bool _isContentFound = true;
        protected string upcomingBikesLink;
        protected string articleUrl = string.Empty, articleTitle = string.Empty, basicId = string.Empty, authorName = string.Empty, displayDate = string.Empty;
        protected int makeId;
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
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];

            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();


            ProcessQS();
            GetRoadtestDetails();
            BindPageWidgets();

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

                if (!_basicId.Equals(basicId))
                {
                    // Modified By :Lucky Rathore on 12 July 2016.
                    Form.Action = Request.RawUrl;
                    string _newUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
                    var _titleStartIndex = _newUrl.LastIndexOf('/') + 1;
                    var _titleEndIndex = _newUrl.LastIndexOf('-');
                    string _newUrlTitle = _newUrl.Substring(_titleStartIndex, _titleEndIndex - _titleStartIndex + 1);
                    _newUrl = "/expert-reviews/" + _newUrlTitle + basicId + ".html";
                    CommonOpn.RedirectPermanent(_newUrl);
                }
            }
            else
            {
                Response.Redirect("/expert-reviews/", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
        }

        private void GetRoadtestDetails()
        {
            try
            {

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                           .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                           .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _objArticles = container.Resolve<ICMSCacheContent>();

                    objRoadtest = _objArticles.GetArticlesDetails(Convert.ToUInt32(_basicId));

                    if (objRoadtest != null)
                    {
                        BindPages();
                        GetRoadtestData();
                        GetTaggedBikeList();
                        BindGallery(objRoadtest.Title);

                    }
                    else
                    {
                        _isContentFound = false;
                    }
                }


            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (!_isContentFound)
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : To bind model gallery
        /// </summary>
        private void BindGallery(string articleTitle)
        {
            try
            {

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>()
                               .RegisterType<ICMSCacheContent, CMSCacheRepository>()
                               .RegisterType<ICacheManager, MemcacheManager>();
                    ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

                    IEnumerable<ModelImage> objImg = _cache.GetArticlePhotos(Convert.ToInt32(_basicId));

                    if (objImg != null && objImg.Count() > 0)
                    {
                        ctrPhotoGallery.BasicId = Convert.ToInt32(_basicId);
                        ctrPhotoGallery.ModelImageList = objImg;
                        ctrPhotoGallery.BindPhotos();

                        ctrlModelGallery.bikeName = articleTitle;
                        ctrlModelGallery.Photos = objImg.ToList();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : To get tagged bike along with article
        /// </summary>
        private void GetTaggedBikeList()
        {
            if (objRoadtest != null && objRoadtest.VehiclTagsList.Count > 0)
            {

                var taggedMakeObj = objRoadtest.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.MakeBase.MaskingName));
                if (taggedMakeObj != null)
                {
                    _taggedMakeObj = taggedMakeObj.MakeBase;
                }
                else
                {
                    _taggedMakeObj = objRoadtest.VehiclTagsList.FirstOrDefault().MakeBase;
                    FetchMakeDetails();
                }
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar on 10th Nov 2016
        /// Description : To get make details by id
        /// </summary>
        private void FetchMakeDetails()
        {

            try
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
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.ViewRT.FetchMakeDetails");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 8 Nov 2016
        /// Summary  : Bind upcoming bikes list
        /// </summary>
        /// </summary>
        private void BindPageWidgets()
        {
            currentCityArea = GlobalCityArea.GetGlobalCityArea();

            if (ctrlPopularBikes != null)
            {


                ctrlPopularBikes.totalCount = 3;
                ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                ctrlPopularBikes.cityName = currentCityArea.City;

                ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                ctrlUpcomingBikes.pageSize = 9;
                ctrlUpcomingBikes.topCount = 3;


                if (_taggedMakeObj != null)
                {
                    ctrlPopularBikes.makeId = _taggedMakeObj.MakeId;
                    ctrlPopularBikes.makeName = _taggedMakeObj.MakeName;
                    ctrlPopularBikes.makeMasking = _taggedMakeObj.MaskingName;
                    ctrlUpcomingBikes.makeMaskingName = _taggedMakeObj.MaskingName;
                    ctrlUpcomingBikes.MakeId = _taggedMakeObj.MakeId;
                    ctrlUpcomingBikes.makeName = _taggedMakeObj.MakeName;

                }
            }
            

        }
        private void BindPages()
        {
            rptPages.DataSource = objRoadtest.PageList;
            rptPages.DataBind();

            rptPageContent.DataSource = objRoadtest.PageList;
            rptPageContent.DataBind();
        }


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