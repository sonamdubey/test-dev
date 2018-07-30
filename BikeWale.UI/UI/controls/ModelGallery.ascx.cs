using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.CMS.Photos;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    public class ModelGallery : System.Web.UI.UserControl
    {
        protected Repeater rptVideoNav, rptModelPhotos, rptNavigationPhoto;
        public string bikeName = String.Empty;
        public int imageCount = 0, videoCount = 0;
        public int modelId;
        public List<ModelImage> Photos;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Photos != null && Photos.Count > 1)
            {
                BindModelGalleryWidget();
            }
        }

        private void BindModelGalleryWidget()
        {
            BindModelGallery bmg = new BindModelGallery();
            bmg.ModelId = modelId;
            bmg.BindImages(rptModelPhotos, rptNavigationPhoto, Photos);
            bmg.BindVideos(rptVideoNav);
            videoCount = bmg.FetchedVideoCount;
        }

        public override void Dispose()
        {
            rptVideoNav.DataSource = null;
            rptVideoNav.Dispose();

            rptModelPhotos.DataSource = null;
            rptModelPhotos.Dispose();

            rptNavigationPhoto.DataSource = null;
            rptNavigationPhoto.Dispose();

            base.Dispose();
        }
    }
}