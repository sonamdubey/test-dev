using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 25 Jan 2017
    /// Summary    : Bind list of top popular bikes by category
    /// </summary>
    public class PopularBikesByBodyStyle : System.Web.UI.UserControl
    {
        public int topCount { get; set; }
        public uint ModelId { get; set; }
        public uint CityId { get; set; }
        public int FetchedRecordsCount { get; set; }
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
                FetchedRecordsCount = objPopular.FetchedRecordsCount;
            }
        }

    }
}