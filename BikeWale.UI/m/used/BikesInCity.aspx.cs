using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Used;
using System;
using System.Collections.Generic;

namespace Bikewale.Mobile.Used
{
    public partial class BikesInCity : System.Web.UI.Page
    {
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
        }
    }
}