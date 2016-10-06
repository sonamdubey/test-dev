using Bikewale.Common;
using Bikewale.DAL.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Used
{
    public class Default : System.Web.UI.Page
    {
        public IEnumerable<CityEntityBase> cities = null;

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
            GetAllCities();
        }

        private void GetAllCities()
        {
            ICity _city = new CityRepository();
            try
            {
                cities = _city.GetAllCities(EnumBikeType.Used);
            }
            catch(Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Exception : GetAllCities - used-Default");
                objErr.SendMail();
            }
        }

    }
}