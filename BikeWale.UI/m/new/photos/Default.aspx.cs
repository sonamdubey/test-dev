using Bikewale.BindViewModels.Webforms.Photos;
using Bikewale.Mobile.Controls;
using System;
using System.Web;

namespace Bikewale.Mobile.New.Photos
{
    /// <summary>
    /// Created By : Sushil Kumar on 5th Jan 2017
    /// Description : Added new page for photos page and bind modelgallery,videos and generic bike info widgets
    /// </summary>
    public class Default : System.Web.UI.Page
    {

        protected ModelGallery ctrlModelGallery;
        protected NewVideosWidget ctrlVideos;
        protected BindModelPhotos vmModelPhotos = null;
        protected MinGenericBikeInfoControl ctrlGenericBikeInfo;
        protected SimilarBikeWithPhotos ctrlSimilarBikesWithPhotos;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindPhotosPage();
        }

        private void BindPhotosPage()
        {
            try
            {
                vmModelPhotos = new BindModelPhotos();
                if (!vmModelPhotos.isRedirectToModelPage && !vmModelPhotos.isPermanentRedirection && !vmModelPhotos.isPageNotFound)
                {
                    vmModelPhotos.GetModelDetails();
                    BindModelPhotosPageWidgets();
                }
                else if (vmModelPhotos.isRedirectToModelPage)  ///new/ page for photos exception
                {
                    Response.Redirect("/m/new/", true);
                }
                else if (vmModelPhotos.isPermanentRedirection) //301 redirection
                {
                    Bikewale.Common.CommonOpn.RedirectPermanent(vmModelPhotos.pageRedirectUrl);
                }
                else  //page not found
                {
                    Response.Redirect("/pagenotfound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Mobile.New.Photos : BindPhotosPage");
            }
        }

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

                ctrlSimilarBikesWithPhotos.TotalRecords = 6;
                ctrlSimilarBikesWithPhotos.ModelId = vmModelPhotos.objModel.ModelId;

                ctrlGenericBikeInfo.ModelId = (uint)vmModelPhotos.objModel.ModelId;
            }

        }
    }
}