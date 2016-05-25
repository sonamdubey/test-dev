using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Cache.Core;
using Bikewale.Controls;
using Bikewale.Interfaces.Cache.Core;
using Enyim.Caching;
using Microsoft.Practices.Unity;

namespace Bikewale
{
    public partial class test : System.Web.UI.Page
    {        
        protected ModelPriceInNearestCities ctrlTopCityPrices;

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlTopCityPrices.ModelId = Convert.ToUInt32(99);
            ctrlTopCityPrices.CityId = 1;
            ctrlTopCityPrices.TopCount = 8;
        }
    }
}