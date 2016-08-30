using Bikewale.Entities.Used;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    public class UsedBikePhotoGallery : System.Web.UI.UserControl
    {
        protected Repeater rptUsedBikeNavPhotos, rptUsedBikePhotos;
        public string BikeName { get; set; }
        public string ModelYear { get; set; }
        public uint PhotosCount { get; set; }
        public IList<BikePhoto> Photos { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Photos != null && Photos.Count >= 1)
            {
                BindUsedBikePhotosWidget();
            }
        }

        private void BindUsedBikePhotosWidget()
        {
            if (PhotosCount > 0)
            {
                rptUsedBikePhotos.DataSource = Photos;
                rptUsedBikePhotos.DataBind();

                rptUsedBikeNavPhotos.DataSource = Photos;
                rptUsedBikeNavPhotos.DataBind();

            }

        }
    }
}