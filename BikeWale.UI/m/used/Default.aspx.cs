using Bikewale.BindViewModels.Controls;
using Bikewale.Common;
using Bikewale.Entities.Used;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Mobile.Used
{
    public partial class Default : System.Web.UI.Page
    {
        protected IEnumerable<UsedBikeCities> objBikeCityCount = null;
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
                objBikeCityCount = objBikeCityCount.Take(6);

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Default.BindCities");
                objErr.SendMail();
            }
        }
    }
}