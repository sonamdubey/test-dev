using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By : Aditi Srivastava on 16 Nov 2016
    /// Summary    : To inject popular bikes widget for cms pages
    /// </summary>
    public class PopularBikesMin : System.Web.UI.UserControl
    {
        public int? totalCount { get; set; }
        public int sortBy { get; set; }
        public int pageSize { get; set; }
        public int makeId { get; set; }
        public int FetchedRecordsCount { get; set; }

        public string makeName = string.Empty;
        public string makeMasking = string.Empty;

        public int CityId { get; set; }
        public string cityName = string.Empty;

        public IEnumerable<MostPopularBikesBase> objPopularBikes = null;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PopularBikes();
        }

        /// <summary>
        /// Created By : Aditi Srivastava on 16 Nov 2016
        /// Summary    : To inject popular bikes widget for cms pages
        /// </summary>
        private void PopularBikes()
        {
            try
            {
                BindMostPopularBikesControl objPop = new BindMostPopularBikesControl();
                objPop.totalCount = totalCount.HasValue && totalCount.Value > 0 ? totalCount : 4;
                objPop.makeId = makeId;
                objPop.cityId = CityId;
                if (CityId > 0)
                {
                    objPop.BindMostPopularBikesMakeCity(null);

                }
                else
                {
                    objPop.BindMostPopularBikes(null);
                }
                objPopularBikes = objPop.popularBikes;
                FetchedRecordsCount = objPop.FetchedRecordsCount;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "PopularBikesMin.PopularBikes");
                
            }
        }


    }
}