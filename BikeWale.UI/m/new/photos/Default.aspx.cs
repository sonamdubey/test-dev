using Bikewale.BindViewModels.Controls;
using Bikewale.BindViewModels.Webforms.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.PriceQuote;
using Bikewale.Mobile.Controls;
using System;
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
        protected bool IsDiscontinued { get; set; }
        protected bool isModelPage;
        protected uint VideoCount;
        protected PQSourceEnum pqSource;
        protected string bikeUrl = string.Empty, bikeName = string.Empty;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        /// <summary>
        /// Modified By :- Subodh Jain 20 jan 2017
        /// Summary :- model page photo bind condition added in query string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(Request.QueryString["modelpage"]))
            {
                isModelPage = true;
            }
            BindPhotosPage();


        }

        /// <summary>
        /// Created By : Sushil Kumar on 6th Jan 2017
        /// Description : Bind photos page with metas,photos and widgets
        /// Modified By :- Subodh jain 20 jan 2017
        /// Summary :- Added ismodel page flag for gallery binding
        /// modified by :- Subodh Jain 30 jan 2017
        /// Summary:- Added model gallery info values and added URL formatter
        /// </summary>
        private void BindPhotosPage()
        {
            try
            {
                vmModelPhotos = new BindModelPhotos();
                if (!vmModelPhotos.isRedirectToModelPage && !vmModelPhotos.isPermanentRedirection && !vmModelPhotos.isPageNotFound)
                {
                    vmModelPhotos.isModelpage = isModelPage;
                    vmModelPhotos.GetModelDetails();
                    IsDiscontinued = vmModelPhotos.IsDiscontinued;
                    BindModelPhotosPageWidgets();
                    BindGenericBikeInfo genericBikeInfo = new BindGenericBikeInfo();

                    if (vmModelPhotos.objModel != null)
                    {
                        genericBikeInfo.ModelId = (uint)vmModelPhotos.objModel.ModelId;
                    }
                    bikeInfo = genericBikeInfo.GetGenericBikeInfo();

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
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Mobile.New.Photos : BindPhotosPage");
            }
            finally
            {
                if (vmModelPhotos.isRedirectToModelPage)  ///new/ page for photos exception
                {
                    Response.Redirect("/m/new-bikes-in-india/", true);
                }
                else if (vmModelPhotos.isPermanentRedirection) //301 redirection
                {
                    Bikewale.Common.CommonOpn.RedirectPermanent(vmModelPhotos.pageRedirectUrl);
                }
                else if (vmModelPhotos.isPageNotFound)  //page not found
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 6th Jan 2017
        /// Description : bind photos page widgets
        /// </summary>
        private void BindModelPhotosPageWidgets()
        {
            if (vmModelPhotos.objMake != null && vmModelPhotos.objModel != null)
            {
                ctrlVideos.TotalRecords = 3;
                ctrlVideos.MakeMaskingName = vmModelPhotos.objMake.MaskingName;
                ctrlVideos.ModelMaskingName = vmModelPhotos.objModel.MaskingName;
                ctrlVideos.ModelId = vmModelPhotos.objModel.ModelId;
                ctrlVideos.MakeName = vmModelPhotos.objMake.MakeName;
                ctrlVideos.ModelName = vmModelPhotos.objModel.ModelName;

                if (!IsDiscontinued)
                {
                    ctrlSimilarBikesWithPhotos.TotalRecords = 6;
                    ctrlSimilarBikesWithPhotos.ModelId = vmModelPhotos.objModel.ModelId;
                }

                ctrlGenericBikeInfo.ModelId = (uint)vmModelPhotos.objModel.ModelId;


            }

        }
    }
}
