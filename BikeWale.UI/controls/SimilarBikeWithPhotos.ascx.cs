using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Sajal Gupta on 09-02-2017
    /// Description : Control to bind similar bikes with photos count 
    /// Created By : Snehal Dange on 07-07-2017
    /// Description : Added CityId City , SimilarMakeName, SimilarModelName
    /// </summary>
    public class SimilarBikeWithPhotos : System.Web.UI.UserControl
    {
        public ushort TotalRecords { get; set; }
        public int ModelId { get; set; }

        public string SimilarMakeName { get; set; }
        public string SimilarModelName { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string WidgetHeading { get; set; }
        public uint CityId { get; set; }
        public string City { get; set; }

        protected IEnumerable<SimilarBikesWithPhotos> objSimilarBikes = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Created By : Sajal Gupta on 09-02-2017
        /// Description : Control to bind similar bikes with photos count 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (ModelId > 0)
                {
                    BindSimilarBikesWithPhotos vmSimilarBikes = new BindSimilarBikesWithPhotos();
                    vmSimilarBikes.TotalRecords = TotalRecords;
                    vmSimilarBikes.ModelId = ModelId;
                    vmSimilarBikes.CityId = CityId;
                    vmSimilarBikes.City = City;

                    objSimilarBikes = vmSimilarBikes.SimilarBikesWithPhotosCount();

                    if (objSimilarBikes != null)
                    {
                        FetchedRecordsCount = objSimilarBikes.Count();
                        var firstModel = objSimilarBikes.First();
                        var bodyStyle = (firstModel.BodyStyle.Equals((sbyte)EnumBikeBodyStyles.Scooter) ? "Scooters" : "Bikes");
                        WidgetHeading = string.Format("{0} similar to {1} {2}", bodyStyle, SimilarMakeName, SimilarModelName);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Controls : Page_Load");
            }
        }
    }
}