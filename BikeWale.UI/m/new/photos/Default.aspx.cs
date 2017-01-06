using Bikewale.BindViewModels.Webforms.Photos;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using System;
using System.Collections.Generic;

namespace Bikewale.Mobile.New.Photos
{
    /// <summary>
    /// Created By : Sushil Kumar on 5th Jan 2017
    /// Description : Added new page for photos page and bind modelgallery,videos and generic bike info widgets
    /// </summary>
    public class Default : System.Web.UI.Page
    {

        //protected ModelGallery ctrlModelGallery;
        protected string bikeName = string.Empty, modelName = string.Empty, makeName = string.Empty, makeMaskingName = string.Empty, modelMaskingName = string.Empty, modelImage = string.Empty;
        protected int totalImages = 0, remainingImages = 0, modelId = 0, imgCount = 0;
        protected List<ModelImage> objImageList = null;
        protected BikeMakeEntityBase objMake = null;
        protected BindModelPhotos vmModelPhotos = null;

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
                    Response.Redirect(Bikewale.Common.CommonOpn.AppPath + "pageNotFound.aspx", true);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Mobile.New.Photos : BindPhotosPage");
            }
        }

        private void BindModelPhotosPageWidgets()
        {

        }
    }
}