using Bikewale.BindViewModels.Controls;
using Bikewale.BindViewModels.Webforms.Photos;
using Bikewale.Common;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using System;
using System.Collections.Specialized;
using System.Web;

namespace Bikewale.Mobile.New.Photos
{
    /// <summary>
    /// Created By : Sushil Kumar on 6th Jan 2017
    /// Description : Added new page for photos page and bind modelgallery,videos and generic bike info widgets
    /// Modified by : Aditi Srivastava on 23 Jan 2017
    /// summary     : Removed Isupcoming flag(added in common viewmodel) 
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected GenericBikeInfo bikeInfo;
        protected NewVideosWidget ctrlVideos;
        protected BindModelPhotos vmModelPhotos = null;
        protected GenericBikeInfoControl ctrlGenericBikeInfo;
        protected SimilarBikeWithPhotos ctrlSimilarBikesWithPhotos;
        protected bool IsUpcoming { get; set; }
        protected uint modelId;
        protected bool IsDiscontinued { get; set; }
        protected uint VideoCount, colorImageId = 0;
        protected PQSourceEnum pqSource;
        protected string bikeUrl = string.Empty, bikeName = string.Empty, returnUrl = string.Empty;
        protected string JSONImageList = string.Empty, JSONVideoList = string.Empty;
        protected uint imageIndex = 0;
        private string queryString = string.Empty;
        public CityEntityBase CityDetails;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        /// <summary>
        /// Modified By :- Subodh Jain 20 jan 2017
        /// Summary :- model page photo bind condition added in query string
        /// Modified by : Sajal Gupta on 16-03-2017
        /// Description : Fetch color image id from query srting.
        /// MOdiefied By :- Subodh Jain 2 may 2017
        /// Description :- Added city details entity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["q"]))
            {
                queryString = EncodingDecodingHelper.DecodeFrom64(Request.QueryString["q"]);
            }

            ProcessQueryStringVariables();
            BindPhotosPage();
            GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
            uint cityId = currentCityArea.CityId;

            if (cityId > 0)
                CityDetails = new CityHelper().GetCityById(cityId);
        }

        /// <summary>
        /// Created by Sajal Gupta on 27-04-2017
        /// Description : Function to get query string variables
        /// </summary>
        private void ProcessQueryStringVariables()
        {
            try
            {
                NameValueCollection queryCollection = HttpUtility.ParseQueryString(queryString);
                uint.TryParse(queryCollection["imageindex"], out imageIndex);
                uint.TryParse(queryCollection["colorImageId"], out colorImageId);
                returnUrl = queryCollection["retUrl"];
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Mobile.New.Photos : ProcessQueryStringVariables");
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 6th Jan 2017
        /// Description : Bind photos page with metas,photos and widgets        
        /// Modified By :- Subodh jain 20 jan 2017
        /// Summary :- Added ismodel page flag for gallery binding
        /// modified by :- Subodh Jain 30 jan 2017
        /// Summary:- Added model gallery info values and added URL formatter
        /// Modified By:Sajal Gupta on 28-02-2017
        /// Description : Dump imagelist and videolist 
        /// </summary>
        private void BindPhotosPage()
        {
            try
            {
                vmModelPhotos = new BindModelPhotos();

                if (!vmModelPhotos.isRedirectToModelPage && !vmModelPhotos.isPermanentRedirection && !vmModelPhotos.isPageNotFound)
                {
                    vmModelPhotos.GridSize = 30;
                    vmModelPhotos.NoOfGrid = 6;

                    vmModelPhotos.GetPhotoGalleryData();

                    IsDiscontinued = vmModelPhotos.IsDiscontinued;
                    BindModelPhotosPageWidgets();
                    BindBikeInfo genericBikeInfo = new BindBikeInfo();

                    if (vmModelPhotos.objModel != null)
                    {
                        genericBikeInfo.ModelId = (uint)vmModelPhotos.objModel.ModelId;
                    }
                    bikeInfo = genericBikeInfo.GetBikeInfo();

                    if (bikeInfo != null)
                    {
                        if (bikeInfo.Make != null && bikeInfo.Model != null)
                        {
                            bikeUrl = string.Format("/m{0}", Bikewale.Utility.UrlFormatter.BikePageUrl(bikeInfo.Make.MaskingName, bikeInfo.Model.MaskingName));
                            bikeName = string.Format("{0} {1}", bikeInfo.Make.MakeName, bikeInfo.Model.ModelName);
                        }
                        pqSource = PQSourceEnum.Mobile_GenricBikeInfo_Widget;
                        IsUpcoming = genericBikeInfo.IsUpcoming;
                        IsDiscontinued = genericBikeInfo.IsDiscontinued;
                        VideoCount = bikeInfo.VideosCount;
                        JSONImageList = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64((new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(vmModelPhotos.objImageList));
                        JSONVideoList = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64((new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(vmModelPhotos.objVideosList));
                    }
                    
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Mobile.New.Photos : BindPhotosPage");
            }
            finally
            {
                if (vmModelPhotos.isPermanentRedirection) //301 redirection
                {
                    Bikewale.Common.CommonOpn.RedirectPermanent(vmModelPhotos.pageRedirectUrl);
                }
                else if (vmModelPhotos.isRedirectToModelPage)  ///new/ page for photos exception
                {
                    Response.Redirect("/m/new-bikes-in-india/", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                else if (vmModelPhotos.isPageNotFound)  //page not found
                {
                    UrlRewrite.Return404();

                }
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 6th Jan 2017
        /// Description : bind photos page widgets
        /// Modified by  : Sajal Gupta on 31-01-2017
        /// Description : Fetch modlId.
        /// Modified  By :- subodh Jain 10 Feb 2017
        /// Summary :- BikeInfo Slug details
        /// Modified by :Snehal Dange on 8th Sep,2017
        /// Description : Added CityId, SimilarMakeName,  SimilarModelName
        /// </summary>                 
        private void BindModelPhotosPageWidgets()
        {
            if (vmModelPhotos.objMake != null && vmModelPhotos.objModel != null)
            {
                modelId = (uint)vmModelPhotos.objModel.ModelId;
                if (ctrlVideos != null)
                {
                    ctrlVideos.TotalRecords = 3;
                    ctrlVideos.MakeMaskingName = vmModelPhotos.objMake.MaskingName;
                    ctrlVideos.ModelMaskingName = vmModelPhotos.objModel.MaskingName;
                    ctrlVideos.ModelId = vmModelPhotos.objModel.ModelId;
                    ctrlVideos.MakeName = vmModelPhotos.objMake.MakeName;
                    ctrlVideos.ModelName = vmModelPhotos.objModel.ModelName;

                    if (!IsDiscontinued)
                    {
                        GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();

                        ctrlSimilarBikesWithPhotos.TotalRecords = 9;
                        ctrlSimilarBikesWithPhotos.City = currentCityArea.City;
                        ctrlSimilarBikesWithPhotos.CityId = currentCityArea.CityId;
                        ctrlSimilarBikesWithPhotos.ModelId = vmModelPhotos.objModel.ModelId;
                        ctrlSimilarBikesWithPhotos.SimilarMakeName = vmModelPhotos.objMake.MakeName;
                        ctrlSimilarBikesWithPhotos.SimilarModelName = vmModelPhotos.objModel.ModelName;

                    }
                    if (ctrlGenericBikeInfo != null)
                    {
                        ctrlGenericBikeInfo.ModelId = (uint)vmModelPhotos.objModel.ModelId;
                        ctrlGenericBikeInfo.CityId = GlobalCityArea.GetGlobalCityArea().CityId;
                        ctrlGenericBikeInfo.PageId = BikeInfoTabType.Image;
                        ctrlGenericBikeInfo.TabCount = 3;
                    }

                }
            }
        }

    }
}
