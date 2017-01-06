using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Mobile.Controls
{
    public class SimilarBikeWithPhotos : System.Web.UI.UserControl
    {
        public ushort TotalRecords { get; set; }
        public int ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }
        protected IEnumerable<SimilarBikesWithPhotos> objSimilarBikes = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ModelId > 0)
            {
                BindSimilarBikesWithPhotos vmSimilarBikes = new BindSimilarBikesWithPhotos();
                if (vmSimilarBikes != null)
                {
                    vmSimilarBikes.TotalRecords = TotalRecords;
                    vmSimilarBikes.ModelId = ModelId;
                    objSimilarBikes = vmSimilarBikes.SimilarBikesWithPhotosCount();
                    if (objSimilarBikes != null)
                        FetchedRecordsCount = objSimilarBikes.Count();
                }
            }
        }
    }
}