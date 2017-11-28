using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By :- Subodh jain 6 feb 2017
    /// Summary :- Popular Bike by Type
    /// </summary>
    public class PopularBikeByBodyStyleCarousal : System.Web.UI.UserControl
    {
        public int topCount { get; set; }
        public uint ModelId { get; set; }
        public uint CityId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string BodyStyleText { get; set; }
        public string BodyStyleLinkTitle { get; set; }
        public EnumBikeBodyStyles BodyStyle { get; set; }
        public ICollection<MostPopularBikesBase> popularBikes = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PopularBikesByType();
        }
        /// <summary>
        /// Created By :- Subodh jain 6 feb 2017
        /// Summary :- Popular Bike by Type
        /// </summary>
        private void PopularBikesByType()
        {
            try
            {
                BindPopularBikesByBodyStyle objPopular = new BindPopularBikesByBodyStyle();
                objPopular.TopCount = topCount;
                objPopular.ModelId = ModelId;
                objPopular.CityId = CityId;
                if (objPopular.ModelId > 0)
                {
                    popularBikes = objPopular.GetPopularBikesByCategory();
                    FetchedRecordsCount = objPopular.FetchedRecordsCount;
                    if (FetchedRecordsCount > 0)
                    {
                        var bike = popularBikes.FirstOrDefault();
                        BodyStyle = bike != null ? bike.BodyStyle : 0;
                        BodyStyleText = Bikewale.Utility.BodyStyleLinks.BodyStyleHeadingText(BodyStyle);
                        BodyStyleLinkTitle = Bikewale.Utility.BodyStyleLinks.BodyStyleFooterLink(BodyStyle);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Controls.PopularBikesByType");
            }
        }

    }
}