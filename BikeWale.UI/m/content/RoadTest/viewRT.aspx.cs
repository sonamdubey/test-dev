using Bikewale.BAL.BikeData;
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
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Content
{
    /// <summary>
    /// Created By : Ashwini Todkar on 22 May 2014
    /// Modified By: Aditi Srivastava on 31 Jan 2017
    /// Summary    : Added common view model for fetching data and modified popular widgets
    /// </summary>
    public class viewRT : System.Web.UI.Page
    {
        protected HtmlSelect ddlPages;
        protected Repeater rptPageContent;
        protected ModelGallery photoGallery;
        protected GlobalCityAreaEntity currentCityArea;
        protected UpcomingBikesMin ctrlUpcomingBikes;
        protected PopularBikesMin ctrlPopularBikes;
        protected uint BasicId = 0, pageId = 1;
        protected String baseUrl = String.Empty, pageTitle = String.Empty, modelName = String.Empty, modelUrl = String.Empty;
        protected String data = String.Empty, nextPageUrl = String.Empty, prevPageUrl = String.Empty, author = String.Empty, displayDate = String.Empty, canonicalUrl = String.Empty;
        protected StringBuilder _bikeTested;
        protected Repeater rptPhotos;
        protected ArticlePageDetails objRoadtest;
        protected IEnumerable<ModelImage> objImg = null;
        private BikeMakeEntityBase _taggedMakeObj;
        protected uint taggedModelId;
        protected bool isModelTagged;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        protected RoadTestDetails objArticle;
       
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Action = Request.RawUrl;

            if (!IsPostBack)
            {
                BindExpertReviewdetails();
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 30 Jan 2017
        /// Summary    : Bind road test details on page
        /// </summary>
        private void BindExpertReviewdetails()
        {
            try
            {
                objArticle = new RoadTestDetails();
                if (!objArticle.IsPermanentRedirect || !objArticle.IsPageNotFound)
                {
                    objArticle.GetRoadTestDetails();

                    if (objArticle.IsContentFound)
                    {
                        BasicId = objArticle.BasicId;
                        objRoadtest = objArticle.objRoadtest;
                        _taggedMakeObj = objArticle.taggedMakeObj;
                        if (objArticle.taggedModelObj != null)
                            taggedModelId = (uint)objArticle.taggedModelObj.ModelId;
                        if (objRoadtest != null)
                        {
                            GetRoadTestData();
                            if (objRoadtest.PageList != null)
                            {
                                rptPageContent.DataSource = objRoadtest.PageList;
                                rptPageContent.DataBind();
                            }
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
                        BindPageWidgets();
                    }
                    else
                    {
                        Response.Redirect("/m/expert-reviews/", false);
                        if (HttpContext.Current != null)
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Page.Visible = false;
                    }
                }
                else if(objArticle.IsPageNotFound)
                {
                    Response.Redirect("/m/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : Bikewale.Content.viewRT.BindExpertReviewdetails");
            }
            finally
            {
                if (objArticle.IsPermanentRedirect)
                {
                    string newUrl = string.Format("/m/expert-reviews/{0}-{1}.html", objArticle.MappedCWId, Request["t"]);
                    Bikewale.Common.CommonOpn.RedirectPermanent(newUrl);
                }
            }
        }

       
        /// <summary>
        /// Modified by : Aditi Srivastava on 22 Nov 2016
        /// Description : To get masking name of tagged make for url if it is null
        /// Modified by : Sajal Gupta on 27-01-2017
        /// Description : Saved taggedModelId  in the variable.
        /// Modified by : Aditi Srivastava on 30 Jan 2017
        /// Summary     : Used tagged list from common view model
        /// </summary>
        private void GetRoadTestData()
        {
            try
            {
                baseUrl = string.Format("/m/expert-reviews/{0}-{1}/", objRoadtest.ArticleUrl, BasicId);
                canonicalUrl = string.Format("https://www.bikewale.com/expert-reviews/{0}-{1}.html", objRoadtest.ArticleUrl, BasicId);
                data = objRoadtest.Description;
                author = objRoadtest.AuthorName;
                pageTitle = objRoadtest.Title;
                displayDate = objRoadtest.DisplayDate.ToString();

                if (objRoadtest.VehiclTagsList != null && objRoadtest.VehiclTagsList.Count > 0)
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
                                _bikeTested.Append(string.Format("<a title={0} {1} Bikes href=/m/{2}-bikes/{3}/>{4}</a>", item.MakeBase.MakeName, item.ModelBase.ModelName, item.MakeBase.MaskingName, item.ModelBase.MaskingName, item.ModelBase.ModelName));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : Bikewale.Content.viewRT.GetRoadTestData");
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 16 Nov 2016
        /// Description: Bind upcoming and popular bikes
        /// Modified By : Sajal Gupta on 31-01-2017
        /// Description : Binded popular bikes widget
        /// Modified By: Aditi Srivastava on 31 Jan 2017
        /// Summary    : Modified logic for widgets for tagged models
        /// </summary>
        protected void BindPageWidgets()
        {
            currentCityArea = GlobalCityArea.GetGlobalCityArea();
            try
            {
                isModelTagged = (taggedModelId > 0);
                if (ctrlPopularBikes != null)
                {
                    ctrlPopularBikes.totalCount = 9;
                    ctrlPopularBikes.CityId = Convert.ToInt32(currentCityArea.CityId);
                    ctrlPopularBikes.cityName = currentCityArea.City;
                    if (_taggedMakeObj != null)
                    {
                        ctrlPopularBikes.makeId = _taggedMakeObj.MakeId;
                        ctrlPopularBikes.makeName = _taggedMakeObj.MakeName;
                        ctrlPopularBikes.makeMasking = _taggedMakeObj.MaskingName;
                    }
                }

                if (isModelTagged)
                {
                    if (ctrlBikesByBodyStyle != null)
                    {
                        ctrlBikesByBodyStyle.ModelId = taggedModelId;
                        ctrlBikesByBodyStyle.topCount = 9;
                        ctrlBikesByBodyStyle.CityId = currentCityArea.CityId;
                    }
                }
                else
                {
                    if (ctrlUpcomingBikes != null)
                    {
                        ctrlUpcomingBikes.sortBy = (int)EnumUpcomingBikesFilter.Default;
                        ctrlUpcomingBikes.pageSize = 9;
                        if (_taggedMakeObj != null)
                        {
                            ctrlUpcomingBikes.MakeId = _taggedMakeObj.MakeId;
                            ctrlUpcomingBikes.makeMaskingName = _taggedMakeObj.MaskingName;
                            ctrlUpcomingBikes.makeName = _taggedMakeObj.MakeName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : Bikewale.Content.viewRT.BindPageWidgets");

            }
        }
     }
}