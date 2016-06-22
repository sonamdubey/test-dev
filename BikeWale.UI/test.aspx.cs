using Bikewale.Controls;
using System;

namespace Bikewale
{
    public class test : System.Web.UI.Page
    {
        protected ModelPriceInNearestCities ctrlTopCityPrices;
        protected Bikewale.Mobile.Controls.DealersCard mobDealer;
        protected Bikewale.Controls.DealerCard deskDealer;
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlTopCityPrices.ModelId = Convert.ToUInt32(99);
            ctrlTopCityPrices.CityId = 1;
            ctrlTopCityPrices.TopCount = 8;

            mobDealer.makeMaskingName = "honda";
            mobDealer.makeName = "Honda";
            mobDealer.MakeId = 7;
            mobDealer.CityId = 1;
            deskDealer.makeMaskingName = "suzuki";
            deskDealer.makeName = "Suzuki";
            deskDealer.MakeId = 12;
            deskDealer.CityId = 1;
        }
    }
}