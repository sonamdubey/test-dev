using Bikewale.Entities.Used;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created by  : Sushil Kumar on 29th August 2016
    /// Description : Used bike photo gallery widget
    /// </summary>
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

        /// <summary>
        /// Created by  : Sushil Kumar on 29th August 2016
        /// Description : Bind used bike photos gallery
        /// </summary>
        private void BindUsedBikePhotosWidget()
        {
            if (PhotosCount > 0 && rptUsedBikePhotos != null && rptUsedBikeNavPhotos != null)
            {
                rptUsedBikePhotos.DataSource = Photos;
                rptUsedBikePhotos.DataBind();

                rptUsedBikeNavPhotos.DataSource = Photos;
                rptUsedBikeNavPhotos.DataBind();

            }

        }
    }
}