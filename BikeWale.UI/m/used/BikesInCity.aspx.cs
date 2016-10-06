using Bikewale.BindViewModels.Controls;
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
    public partial class BikesInCity : System.Web.UI.Page
    {

        public IEnumerable<UsedBikeCities> objBikeCityCountTop = null;
        public IEnumerable<UsedBikeCities> objBikeCityCountRest = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCities();
            }
        }
        private void BindCities()
        {
            BindUsedBikesCityWithCount objBikeCity = new BindUsedBikesCityWithCount();
            IEnumerable<UsedBikeCities> objBikeCityCount = objBikeCity.GetUsedBikeByCityWithCount();
            objBikeCityCountTop = objBikeCityCount.Take(6);
            objBikeCityCountRest = objBikeCityCount.Skip(6);
        }
    }
}