using Bikewale.BindViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.controls
{
    public class ModelGallery : System.Web.UI.UserControl
    {
        protected Repeater rptVideoNav, rptModelPhotos, rptNavigationPhoto;
        public string bikeName = String.Empty;
        public int imageCount = 0, videoCount = 0;
        public int modelId;
        public List<Bikewale.DTO.CMS.Photos.CMSModelImageBase> Photos;

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
            BindModelGallery.ModelId = modelId;
            BindModelGallery.BindImages(rptModelPhotos,rptNavigationPhoto, Photos);
            BindModelGallery.BindVideos(rptVideoNav);            
            videoCount = BindModelGallery.FetchedVideoCount;
        }
    }
}