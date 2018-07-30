using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    public class ModelGallery : System.Web.UI.UserControl
    {
        protected Repeater rptVideoNav;
        public string bikeName = String.Empty, modelName = string.Empty, articleName = string.Empty;
        public int imageCount = 0, videoCount = 0;
        public int modelId;
        public IList<ModelImage> Photos;
        public bool ShowWidget;
        //set this variable to false when model gallery is injected to any other page
        public bool isModelPage = true;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindModelGalleryWidget();
        }
        /// <summary>
        /// Modified By : Subodh Jain 6 Jan 2017
        /// Summary:- Added Check for count and removed repeater for photo binding
        /// Modified By : Sushil Kumar on 27th Jan 2017
        /// Description : Handle null check for photos 
        /// </summary>
        private void BindModelGalleryWidget()
        {
            try
            {
                BindModelGallery ObjBindModelGallery = new BindModelGallery();
                if (ObjBindModelGallery != null)
                {
                    ObjBindModelGallery.ModelId = modelId;
                    ObjBindModelGallery.BindVideos(rptVideoNav);
                    videoCount = ObjBindModelGallery.FetchedVideoCount;
                    ShowWidget = (Photos != null && Photos.Count > 1) || videoCount > 1;
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Mobile.Controls.BindModelGalleryWidget");
            }
        }
    }
}