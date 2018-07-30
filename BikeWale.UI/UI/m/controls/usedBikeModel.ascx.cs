using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.Entities.Location;
using Bikewale.Entities.UsedBikes;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Mobile.Controls
{

    /// Created  By :-Subodh Jain on 15 March 2017
    /// Summary :-Used Bike Widget
    public class UsedBikeModel : System.Web.UI.UserControl
    {
        public uint CityId { get; set; }
        public uint MakeId { get; set; }
        public uint TopCount { get; set; }
        protected IEnumerable<MostRecentBikes> UsedBikeModelInCityList;
        public uint FetchCount;
        public string header { get; set; }
        public string CityMasking { get; set; }
        public CityEntityBase cityDetails;
        public string WidgetTitle { get; set; }
        public string WidgetHref { get; set; }
        public bool IsLandingPage { get; set; }
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Bindwidget();
        }
        /// <summary>
        /// Created By : Subodh Jain on 2 jan 2017 
        /// Description : Bind Used Bike By Models widget
        /// </summary>
        private void Bindwidget()
        {
            try
            {
                BindUsedBikeModelInCity objUsedBikeModelCity = new BindUsedBikeModelInCity();
                if (MakeId > 0)
                {
                    if (CityId > 0)
                        UsedBikeModelInCityList = objUsedBikeModelCity.GetUsedBikeByModelCountInCity(MakeId, CityId, TopCount);
                    else
                        UsedBikeModelInCityList = objUsedBikeModelCity.GetPopularUsedModelsByMake(MakeId, TopCount);
                }
                else
                {
                    if (CityId > 0)
                        UsedBikeModelInCityList = objUsedBikeModelCity.GetUsedBikeCountInCity(CityId, TopCount);
                    else
                        UsedBikeModelInCityList = objUsedBikeModelCity.GetUsedBike(TopCount);

                }
                if (CityId > 0)
                    cityDetails = new CityHelper().GetCityById(CityId);
                if (UsedBikeModelInCityList != null && UsedBikeModelInCityList.Any())
                    FetchCount = (uint)UsedBikeModelInCityList.Count();


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UsedBikeByModels.Bindwidget");
            }

        }
    }
}