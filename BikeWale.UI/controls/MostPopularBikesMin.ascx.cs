using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;

namespace Bikewale.Controls
{
    /// <summary>
    /// Modified By : Sushil Kumar on 10th Nov 2016
    /// Description : Added provision to bind most popular bikes for edit cms
    /// </summary>
    public class MostPopularBikesMin : System.Web.UI.UserControl
    {
        public int? totalCount { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string PageId { get; set; }
        public int PQSourceId { get; set; }
        public int CityId { get; set; }
        public string cityName = string.Empty;

        public int MakeId { get; set; }
        public string makeName = string.Empty;
        public string makeMasking = string.Empty;

        public IEnumerable<MostPopularBikesBase> popularBikes = null;

        protected string popularBikesByMakeLink = string.Empty;

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
            objPop.makeId = MakeId;
            objPop.cityId = CityId;
            if (CityId > 0)
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