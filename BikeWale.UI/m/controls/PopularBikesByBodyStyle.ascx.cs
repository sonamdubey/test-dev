using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 16 Nov 2016
    /// Summary    : To inject upcoming bikes widget for cms pages
    /// </summary>
    public class PopularBikesByBodyStyle : System.Web.UI.UserControl
    {

        public int topCount { get; set; }
        public uint ModelId { get; set; }
        public uint CityId { get; set; }
        public int FetchedRecordsCount { get; set; }
        protected string BodyStyleHeading { get; set; }
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

        private void PopularBikesByType()
        {
            BindPopularBikesByBodyStyle objPopular = new BindPopularBikesByBodyStyle();
            objPopular.TopCount = topCount;
            objPopular.ModelId = ModelId;
            objPopular.CityId = CityId;
            objPopular.BodyStyle = BodyStyle;
            if (objPopular.ModelId > 0)
            {
                popularBikes = objPopular.GetPopularBikesByCategory();
                BodyStyle = objPopular.BodyStyle;
                FetchedRecordsCount = objPopular.FetchedRecordsCount;
                if (FetchedRecordsCount > 0)
                    BodyStyleHeading = popularBikes.FirstOrDefault().CategoryName;
            }
        }
    }
}