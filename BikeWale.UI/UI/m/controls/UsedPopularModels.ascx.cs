using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.UsedBikes;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created by Sangram Nandkhile on 03 Feb 2016
    /// Dec:- Used bike widget to show popular models by Make
    /// </summary>
    public class UsedPopularModels : System.Web.UI.UserControl
    {
        public IEnumerable<MostRecentBikes> UsedBikeModelInCityList;
        public uint MakeId { get; set; }
        public uint TopCount { get; set; }
        public uint CityId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string MakeName { get; set; }
        public string modelName = string.Empty;
        public string CityName { get; set; }
        public string MakeMaskingName { get; set; }
        public string modelMaskingName = string.Empty;
        public string CityMaskingName { get; set; }
        public string pageHeading = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponents();
        }

        void InitializeComponents()
        {
            this.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BindUsedBikes();
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 03 Feb 2016
        /// Description : Function to bind used bikes for the makes/models.          
        /// </summary>
        protected void BindUsedBikes()
        {
            try
            {
                BindUsedBikeModelInCity objUsedBikeModelCity = new BindUsedBikeModelInCity();
                if (MakeId > 0 && CityId > 0)
                {
                    UsedBikeModelInCityList = objUsedBikeModelCity.GetUsedBikeByModelCountInCity(MakeId, CityId, TopCount);
                }
                else
                {
                    UsedBikeModelInCityList = objUsedBikeModelCity.GetPopularUsedModelsByMake(MakeId, TopCount);
                    CityName = "India";
                }
                if (UsedBikeModelInCityList != null && UsedBikeModelInCityList.Any())
                    FetchedRecordsCount = UsedBikeModelInCityList.Count();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Mobile.Controls.UsedPopularModels.BindUsedBikes ==> makeid {0}, cityId {1}, topCount {2}", MakeId, CityId, TopCount));
            }
        }
    }
}