﻿using Bikewale.BAL.BikeData;
using Bikewale.BAL.EditCMS;
using Bikewale.BindViewModels.Webforms.EditCMS;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.CMS;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Memcache;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Bikewale.Mobile.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 22 May 2014
    /// </summary>
    public class viewF : System.Web.UI.Page
    {
        //protected Repeater rptPages, rptPageContent;
        protected Repeater rptPageContent;
        protected UpcomingBikesMin ctrlUpcomingBikes;
        protected PopularBikesMin ctrlPopularBikes;
        protected ModelGallery photoGallery;
        protected HtmlSelect ddlPages;
        protected int BasicId = 0, pageId = 1;
        protected String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty, modelUrl = String.Empty;
        protected String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = string.Empty, url = string.Empty;
        protected Repeater rptPhotos;
        protected ArticlePageDetails objFeature = null;
        protected FeaturesDetails objArticle;
        private BikeMakeEntityBase _taggedMakeObj;
        private BikeModelEntityBase _taggedModelObj;
        protected GlobalCityAreaEntity currentCityArea;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        protected uint modelId;
        protected bool showBodyStyleWidget;
        private bool _isContentFount = true;
        protected IEnumerable<ModelImage> objImg = null;
        protected EnumBikeBodyStyles bodyStyle;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            if (!IsPostBack)
            {

                //if (ProcessQueryString())
                //{
                //    if (BasicId > 0)
                //        GetFeatureDetails();
                //}
                //else
                //{
                //    Response.Redirect("/m/pagenotfound.aspx", false);
                //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                //    this.Page.Visible = false;
                //}
                BindFeaturesDetails();
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 30 Jan 2017
        /// Summary    : Bind feature details on page
        /// </summary>
        private void BindFeaturesDetails()
        {
            try
            {
                objArticle = new FeaturesDetails();
                if (!objArticle.IsPermanentRedirect)
                {
                    if (!objArticle.IsPageNotFound)
                    {
                        objFeature = objArticle.objFeature;
                        _taggedMakeObj = objArticle.taggedMakeObj;
                        _taggedModelObj = objArticle.taggedModelObj;
                        
                        GetFeatureData();
                        BindPages();
                        BindPageWidgets();
                        objImg = objArticle.objImg;

                        if (objImg != null && objImg.Count() > 0)
                        {
                            photoGallery.Photos = objImg.ToList();
                            photoGallery.isModelPage = false;
                            photoGallery.articleName = pageTitle;
                            rptPhotos.DataSource = objImg;
                            rptPhotos.DataBind();
                        }
                    }
                    else if (!objArticle.IsContentFound)
                    {
                        Response.Redirect("/m/features/", false);
                        if (HttpContext.Current != null)
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                    else
                    {
                        Response.Redirect("/m/pagenotfound.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
            }
        }

        //private bool ProcessQueryString()
        //{
        //    bool isSucess = true;

        //    if (Request.QueryString["id"] != null && !String.IsNullOrEmpty(Request.QueryString["id"]) && CommonOpn.CheckId(Request.QueryString["id"]))
        //    {
        //        /** Modified By : Ashwini Todkar on 12 Aug 2014 , add when consuming carwale api
        //        //Check if basic id exists in mapped carwale basic id log **/
        //        string _basicId = BasicIdMapping.GetCWBasicId(Request.QueryString["id"]);

        //        //if id exists then redirect url to new basic id url
        //        if (!_basicId.Equals(Request.QueryString["id"]))
        //        {
        //            string _newUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
        //            var _titleStartIndex = _newUrl.IndexOf('/');
        //            var _titleEndIndex = _newUrl.LastIndexOf('-');
        //            string _newUrlTitle = _newUrl.Substring(_titleStartIndex, _titleEndIndex - _titleStartIndex + 1);
        //            _newUrl = _newUrlTitle + _basicId + "/";
        //            CommonOpn.RedirectPermanent(_newUrl);
        //            Trace.Warn("_newUrl : " + _newUrl);
        //        }

        //        if (!Int32.TryParse(Request.QueryString["id"].ToString(), out BasicId))
        //            isSucess = false;

        //        if (Request.QueryString["pn"] != null && !String.IsNullOrEmpty(Request.QueryString["pn"]) && CommonOpn.CheckId(Request.QueryString["pn"]))
        //        {
        //            if (!Int32.TryParse(Request.QueryString["pn"], out pageId))
        //                isSucess = false;
        //        }
        //    }
        //    else
        //    {
        //        isSucess = false;
        //    }

        //    return isSucess;
        //}
        ///// <summary>
        ///// Modified By : Aditi Srivastava on 17 Nov 2016
        ///// Summary     : Added photo gallery control
        ///// </summary>
        //private void GetFeatureDetails()
        //{
        //    try
        //    {
        //        using (IUnityContainer container = new UnityContainer())
        //        {
        //            container.RegisterType<IArticles, Articles>()
        //                  .RegisterType<ICMSCacheContent, CMSCacheRepository>()
        //                  .RegisterType<ICacheManager, MemcacheManager>();
        //            ICMSCacheContent _cache = container.Resolve<ICMSCacheContent>();

        //            objFeature = _cache.GetArticlesDetails(Convert.ToUInt32(BasicId));

        //            if (objFeature != null)
        //            {

        //                GetFeatureData();
        //                BindPages();
        //                BindPageWidgets();
        //                //objImg = _cache.GetArticlePhotos(BasicId);

        //                if (objImg != null && objImg.Count() > 0)
        //                {
        //                    photoGallery.Photos = objImg.ToList();
        //                    photoGallery.isModelPage = false;
        //                    photoGallery.articleName = pageTitle;
        //                    rptPhotos.DataSource = objImg;
        //                    rptPhotos.DataBind();
        //                }
        //            }
        //            else
        //            {
        //                _isContentFount = false;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.Warn("Ex Message: " + ex.Message);
        //        ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
        //    }
        //    finally
        //    {
        //        if (!_isContentFount)
        //        {
        //            Response.Redirect("/m/pagenotfound.aspx", false);
        //            HttpContext.Current.ApplicationInstance.CompleteRequest();
        //            this.Page.Visible = false;
        //        }
        //    }
        //}

        private void GetFeatureData()
        {


            baseUrl = String.Format("/m/features/{0}-{1}/", objFeature.ArticleUrl, Convert.ToString(objArticle.BasicId));
            //  desktop url for facebook
            url = String.Format("/features/{0}-{1}/", objFeature.ArticleUrl, Convert.ToString(objArticle.BasicId));
            
            author = objFeature.AuthorName;
            pageTitle = objFeature.Title;
            displayDate = Convert.ToDateTime(objFeature.DisplayDate).ToString("MMMM dd, yyyy hh:mm tt");

            if (_taggedModelObj != null)
            {
                modelId = (uint)_taggedModelObj.ModelId;
                modelName = _taggedModelObj.ModelName;
                modelUrl = String.Format("/m/{0}-bikes/{1}/", _taggedMakeObj.MaskingName, _taggedModelObj.MaskingName);
                
            }
                // GetTaggedBikeList();
            //if (objFeature.TagsList != null && objFeature.TagsList.Count > 0)
            //{
            //    if (objFeature.VehiclTagsList != null && objFeature.VehiclTagsList.Count > 0)
            //    {
            //        var modelBase = objFeature.VehiclTagsList[0].ModelBase;
            //        modelName = modelBase.ModelName;
            //        modelId = (uint)modelBase.ModelId;
            //        modelUrl = "/m/" + UrlRewrite.FormatSpecial(objFeature.VehiclTagsList[0].MakeBase.MakeName) + "-bikes/" + objFeature.VehiclTagsList[0].ModelBase.MaskingName + "/";
            //    }
            //}
        }

        private void BindPages()
        {
            if (objFeature.PageList != null)
            {
                rptPageContent.DataSource = objFeature.PageList;
                rptPageContent.DataBind();
            }
        }

        protected string GetImageUrl(string hostUrl, string imagePath)
        {
            string imgUrl = String.Empty;
            imgUrl = Bikewale.Common.ImagingFunctions.GetPathToShowImages(imagePath, hostUrl);

            return imgUrl;
        }

        protected string GetImageUrl(string hostUrl, string imagePath, string size)
        {
            string imgUrl = String.Empty;
            imgUrl = Bikewale.Utility.Image.GetPathToShowImages(imagePath, hostUrl, size);

            return imgUrl;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 16 Nov 2016
        /// Description: bind upcoming and popular bikes
        /// </summary>
        protected void BindPageWidgets()
        {
            currentCityArea = GlobalCityArea.GetGlobalCityArea();
            try
            {
                ctrlPopularBikes.totalCount = 9;
                ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                ctrlPopularBikes.cityName = currentCityArea.City;
                ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                ctrlUpcomingBikes.pageSize = 4;
                if (_taggedMakeObj != null)
                {
                    ctrlPopularBikes.makeId = _taggedMakeObj.MakeId;
                    ctrlPopularBikes.makeName = _taggedMakeObj.MakeName;
                    ctrlPopularBikes.makeMasking = _taggedMakeObj.MaskingName;
                    ctrlUpcomingBikes.MakeId = _taggedMakeObj.MakeId;
                    ctrlUpcomingBikes.makeMaskingName = _taggedMakeObj.MaskingName;
                    ctrlUpcomingBikes.makeName = _taggedMakeObj.MakeName;
                }

                if (modelId > 0)
                {
                    ctrlBikesByBodyStyle.ModelId = modelId;
                    ctrlBikesByBodyStyle.topCount = 9;
                    ctrlBikesByBodyStyle.CityId = currentCityArea.CityId;

                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                            .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                            .RegisterType<ICacheManager, MemcacheManager>()
                            .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>();

                        IBikeModelsCacheRepository<int> modelCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                        bodyStyle = modelCache.GetBikeBodyType(modelId);

                    }

                    if (bodyStyle == EnumBikeBodyStyles.Scooter || bodyStyle == EnumBikeBodyStyles.Cruiser || bodyStyle == EnumBikeBodyStyles.Sports)
                        showBodyStyleWidget = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "viewF.BindPageWidgets");
             }
        }

        ///// <summary>
        ///// Created by : Aditi Srivastava on 16 Nov 2016
        ///// Description: Get details of tagged vehicles
        ///// </summary>
        //private void GetTaggedBikeList()
        //{
        //    if (objFeature != null && objFeature.VehiclTagsList.Count > 0)
        //    {

        //        var taggedMakeObj = objFeature.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.MakeBase.MaskingName));
        //        if (taggedMakeObj != null)
        //        {
        //            _taggedMakeObj = taggedMakeObj.MakeBase;
        //        }
        //        else
        //        {
        //            _taggedMakeObj = objFeature.VehiclTagsList.FirstOrDefault().MakeBase;
        //            FetchMakeDetails();
        //        }
        //    }
        //}
        ///// <summary>
        ///// Created by : Aditi Srivastava on 22 Nov 2016
        ///// Description: fetch make details from tagged list
        ///// </summary>
        //private void FetchMakeDetails()
        //{
        //    try
        //    {
        //        if (_taggedMakeObj != null && _taggedMakeObj.MakeId > 0)
        //        {

        //            using (IUnityContainer container = new UnityContainer())
        //            {
        //                container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
        //                var makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();
        //                _taggedMakeObj = makesRepository.GetMakeDetails(_taggedMakeObj.MakeId.ToString());

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "Bikewale.mobile.viewF.FetchMakeDetails");
        //        objErr.SendMail();
        //    }
        //}

    }
}