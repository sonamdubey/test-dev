using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By : Sushil Kumar on 5th Jan 2017
    /// Description : Control to bind similar bikes with photos count 
    /// </summary>
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
        /// Created By : Sushil Kumar on 5th Jan 2017
        /// Description : Control to bind similar bikes with photos count 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ModelId > 0)
            {
                BindSimilarBikesWithPhotos vmSimilarBikes = new BindSimilarBikesWithPhotos();
                vmSimilarBikes.TotalRecords = TotalRecords;
                vmSimilarBikes.ModelId = ModelId;
                objSimilarBikes = vmSimilarBikes.SimilarBikesWithPhotosCount();
                if (objSimilarBikes != null)
                    FetchedRecordsCount = objSimilarBikes.Count();
            }
        }
    }
}