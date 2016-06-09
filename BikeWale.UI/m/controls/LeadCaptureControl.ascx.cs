using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    public class LeadCaptureControl :  UserControl
    {
        public uint AreaId { get; set; }
        public uint ModelId { get; set; }
        public uint CityId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}