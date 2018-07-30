using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.Entities.Used;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Mobile.Used
{
    /// <summary>
    /// Created by : Subodh Jain 6 oct 2016
    /// Summary: Bind used bikes in a city with count.
    /// </summary>
    public class BikesInCity : System.Web.UI.Page
    {

        protected IEnumerable<UsedBikeCities> objBikeCityCountTop = null;
        protected IEnumerable<UsedBikeCities> objBikeCityCount = null;
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        protected void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {


            BindCities();

        }
        private void BindCities()
        {
            try
            {
                BindUsedBikesCityWithCount objBikeCity = new BindUsedBikesCityWithCount();
                objBikeCityCount = objBikeCity.GetUsedBikeByCityWithCount();
                objBikeCityCountTop = objBikeCityCount.Where(x => x.Priority > 0); ;
                objBikeCityCount = objBikeCityCount.OrderBy(c => c.CityName);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikesInCity.BindCities");
                
            }
        }
    }
}