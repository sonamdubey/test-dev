using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.UsedBikes;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Mobile.Controls
{
    public class UsedBikeModelByCity : System.Web.UI.UserControl
    {
        /// <summary>
        /// Created By : Subodh Jain on 2 jan 2017 
        /// Description : Bind Used Bike By Models 
        /// </summary>
        protected IEnumerable<MostRecentBikes> UsedBikeInCityList;

        public uint CityId { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public string CityMaskingName { get; set; }
        public string CityName { get; set; }
        public uint TopCount { get; set; }
        public int FetchCount;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        protected void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }
        /// <summary>
        /// Created By : Subodh Jain on 2 jan 2017 
        /// Description : Bind Used Bike By Models page_load 
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CityId > 0)
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
                UsedBikeInCityList = objUsedBikeModelCity.GetUsedBikeCountInCity(CityId, TopCount);
                if (UsedBikeInCityList != null && UsedBikeInCityList.Any())
                    FetchCount = UsedBikeInCityList.Count();
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "UsedBikeModelByCity.Bindwidget");
            }

        }
    }
}