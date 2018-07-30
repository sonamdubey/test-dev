using Bikewale.Entities.Location;
using Bikewale.Utility;
using System;
using System.Linq;

namespace Bikewale.Controls
{
    public class PopupWidget : System.Web.UI.UserControl
    {
        public string ClientIP { get { return Bikewale.Common.CommonOpn.GetClientIP(); } }
        public uint CityId { get; set; }
        public uint AreaId { get; set; }
        public uint ModelId { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GlobalCityAreaEntity cityArea = GlobalCityArea.GetGlobalCityArea();
            CityId = cityArea.CityId;
            AreaId = cityArea.AreaId;
        }



    }
}