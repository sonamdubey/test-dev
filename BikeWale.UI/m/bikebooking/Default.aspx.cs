using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Bikewale.BAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Memcache;
using Bikewale.Common;
using System.Text.RegularExpressions;
using System.Data;
using Bikewale.Entities.Customer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.BAL.Customer;
using Bikewale.BAL.PriceQuote;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Net.Http;

namespace Bikewale.Mobile.bikebooking
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Class to get the price quote of the bike.
    /// </summary>
    public class Default : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }   // class
}   // namespace