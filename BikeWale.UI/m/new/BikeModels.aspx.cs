using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.Model;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Mobile.Controls;
using Microsoft.Practices.Unity;
using Bikewale.m.controls;

namespace Bikewale.Mobile.New
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 9 Sept 2015    
    /// </summary>
    public class BikeModels : System.Web.UI.Page
    {
        // Register controls
        protected AlternativeBikes ctrlAlternateBikes;
        protected NewsWidget ctrlNews;
        protected ExpertReviewsWidget ctrlExpertReviews;
        protected VideosWidget ctrlVideos;
        protected UserReviewList ctrlUserReviews;
        protected ModelGallery ctrlModelGallery;
        // Register global variables
        protected ModelPage modelPage;
        protected string modelId = string.Empty;
        protected Repeater rptModelPhotos, rptVarients, rptColors;
        protected String bikeName = String.Empty;
        protected String clientIP = string.Empty;
        protected String cityId = String.Empty;
        protected short reviewTabsCnt = 0;
        //Variable to Assing ACTIVE class
        protected bool isUserReviewActive = false, isExpertReviewActive = false, isNewsActive = false, isVideoActive = false;
        //Varible to Hide or show controlers
        protected bool isUserReviewZero = true, isExpertReviewZero = true, isNewsZero = true, isVideoZero = true;
        protected static bool isManufacturer = false;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        static BikeModels()
        {
            //isManufacturer = (ConfigurationManager.AppSettings["TVSManufacturerId"] != "0") ? true : false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Do not change the sequence
            ParseQueryString();
            CheckCityCookie();
            FetchModelPageDetails();
            #endregion
            if (!IsPostBack)
            {
                #region Do not change the sequence of these functions
                BindRepeaters();
                BindModelGallery();
                BindAlternativeBikeControl();
                clientIP = CommonOpn.GetClientIP();
                #endregion

                ////news,videos,revews, user reviews
                ctrlNews.TotalRecords = 3;
                ctrlNews.ModelId = Convert.ToInt32(modelId);

                ctrlExpertReviews.TotalRecords = 3;
                ctrlExpertReviews.ModelId = Convert.ToInt32(modelId);

                ctrlVideos.TotalRecords = 3;
                ctrlVideos.ModelId = Convert.ToInt32(modelId);

                ctrlUserReviews.ReviewCount = 4;
                ctrlUserReviews.PageNo = 1;
                ctrlUserReviews.PageSize = 4;
                ctrlUserReviews.ModelId = Convert.ToInt32(modelId);

                ctrlExpertReviews.MakeMaskingName = modelPage.ModelDetails.MakeBase.MaskingName.Trim();
                ctrlExpertReviews.ModelMaskingName = modelPage.ModelDetails.MaskingName.Trim();
            }
        }

        private void BindModelGallery()
        {
            List<Bikewale.DTO.CMS.Photos.CMSModelImageBase> photos = null;
            if (modelPage != null && modelPage.Photos != null && modelPage.Photos.Count > 0)
            {
                photos = modelPage.Photos;
                photos.Insert(0, new DTO.CMS.Photos.CMSModelImageBase()
                {
                    HostUrl = modelPage.ModelDetails.HostUrl,
                    OriginalImgPath = modelPage.ModelDetails.OriginalImagePath,
                    ImageCategory = bikeName,
                });
                ctrlModelGallery.bikeName = bikeName;
                ctrlModelGallery.modelId = Convert.ToInt32(modelId);
                ctrlModelGallery.Photos = photos;
            }
        }

        private void BindAlternativeBikeControl()
        {
            ctrlAlternateBikes.TopCount = 6;

            if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
            {
                ctrlAlternateBikes.VersionId = modelPage.ModelVersions[0].VersionId;
            }
        }

        /// <summary>
        /// Function to bind the photos album
        /// </summary>
        private void BindRepeaters()
        {

            if (modelPage != null)
            {
                if (modelPage.Photos != null && modelPage.Photos.Count > 0)
                {
                    //if (modelPage.Photos.Count > 2)
                    //{
                    //    rptModelPhotos.DataSource = modelPage.Photos.Take(3);
                    //}
                    //else
                    //{
                    //    rptModelPhotos.DataSource = modelPage.Photos;
                    //}
                    rptModelPhotos.DataSource = modelPage.Photos;
                    rptModelPhotos.DataBind();
                }

                if (modelPage.ModelVersions != null && modelPage.ModelVersions.Count > 0)
                {
                    rptVarients.DataSource = modelPage.ModelVersions;
                    rptVarients.DataBind();
                }

                if (modelPage.ModelColors != null && modelPage.ModelColors.Count() > 0)
                {
                    rptColors.DataSource = modelPage.ModelColors;
                    rptColors.DataBind();
                }
            }
        }


        /// <summary>
        /// Function to get the required parameters from the query string.
        /// </summary>
        private void ParseQueryString()
        {
            ModelMaskingResponse objResponse = null;
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["model"]))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                        var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();

                        objResponse = objCache.GetModelMaskingResponse(Request.QueryString["model"]);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " : FetchModelPageDetails");
                objErr.SendMail();

                Response.Redirect("/m/new/", true);
            }
            finally
            {
                if (!string.IsNullOrEmpty(Request.QueryString["model"]))
                {
                    if (objResponse != null)
                    {
                        if (objResponse.StatusCode == 200)
                        {
                            modelId = objResponse.ModelId.ToString();
                        }
                        else if (objResponse.StatusCode == 301)
                        {
                            //redirect permanent to new page 
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(Request.QueryString["model"], objResponse.MaskingName));
                        }
                        else
                        {
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", true);
                        }
                    }
                    else
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", true);
                    }
                }
                else
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", true);
                }
            }
        }

        private void CheckCityCookie()
        {
            string location = String.Empty;
            var cookies = this.Context.Request.Cookies;
            if (cookies.AllKeys.Contains("location"))
            {
                location = cookies["location"].Value;
                if (!String.IsNullOrEmpty(location) && location.IndexOf('_') != -1)
                    cityId = location.Substring(0, location.IndexOf('_'));//location.Split('_')[0];
            }
            else
            {
                cityId = "0";
            }
        }

        private void FetchModelPageDetails()
        {
            try
            {
                string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = String.Format("/api/model/details/?modelId={0}", modelId);

                modelPage = BWHttpClient.GetApiResponseSync<ModelPage>(_bwHostUrl, _requestType, _apiUrl, modelPage);

                if (modelPage != null)
                {
                    bikeName = modelPage.ModelDetails.MakeBase.MakeName + ' ' + modelPage.ModelDetails.ModelName;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " : FetchModelPageDetails");
                objErr.SendMail();
            }
        }
    }
}