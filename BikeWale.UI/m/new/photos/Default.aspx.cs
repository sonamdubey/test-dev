using Bikewale.BindViewModels.Webforms.Photos;
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

        protected ModelGallery ctrlModelGallery;
        protected NewVideosWidget ctrlVideos;
        protected BindModelPhotos vmModelPhotos = null;
        protected GenericBikeInfoControl ctrlGenericBikeInfo;
        protected SimilarBikeWithPhotos ctrlSimilarBikesWithPhotos;
        protected bool isDiscontinued = false;
        protected bool isModelPage;
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
                    isDiscontinued = vmModelPhotos.IsDiscontinued;
                    BindModelPhotosPageWidgets();
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
                    Response.Redirect("/m/new/", true);
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

                ctrlModelGallery.bikeName = vmModelPhotos.bikeName;
                ctrlModelGallery.modelName = vmModelPhotos.objModel.ModelName;
                ctrlModelGallery.modelId = vmModelPhotos.objModel.ModelId;
                ctrlModelGallery.Photos = vmModelPhotos.objImageList;

                if (!isDiscontinued)
                {
                    ctrlSimilarBikesWithPhotos.TotalRecords = 6;
                    ctrlSimilarBikesWithPhotos.ModelId = vmModelPhotos.objModel.ModelId;
                }

                ctrlGenericBikeInfo.ModelId = (uint)vmModelPhotos.objModel.ModelId;


            }

        }
    }
}
