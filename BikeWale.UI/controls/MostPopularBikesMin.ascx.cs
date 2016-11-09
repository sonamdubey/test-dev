using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;

namespace Bikewale.Controls
{
    public class MostPopularBikesMin : System.Web.UI.UserControl
    {
        // public Repeater rptMostPopularBikes, rptPopoularBikeMake;
        public int? totalCount { get; set; }
        public int? makeId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string PageId { get; set; }
        public int PQSourceId { get; set; }
        public int? CityId { get; set; }
        public string cityName = string.Empty;
        public string cityMaskingName = string.Empty;
        public string makeName = string.Empty;
        public bool mostPopular = false, mostPopularByMake = false;
        public IEnumerable<MostPopularBikesBase> popularBikes = null;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            MostPopularBikes();
        }

        private void MostPopularBikes()
        {
            BindMostPopularBikesControl objPop = new BindMostPopularBikesControl();
            objPop.totalCount = totalCount.HasValue && totalCount.Value > 0 ? totalCount : 4;
            objPop.makeId = makeId;
            objPop.cityId = CityId;
            if (CityId.HasValue && CityId > 0)
            {
                objPop.BindMostPopularBikesMakeCity(null);

            }
            else
            {
                objPop.BindMostPopularBikes(null);
            }
            popularBikes = objPop.popularBikes;
            FetchedRecordsCount = objPop.FetchedRecordsCount;
        }

    }
}