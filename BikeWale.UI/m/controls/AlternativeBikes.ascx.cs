using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.BindViewModels.Controls;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 9 Sept 2015
    /// Summary : Control to show the alternative bikes of the the specific version
    /// </summary>
    public class AlternativeBikes : System.Web.UI.UserControl
    {
        public Repeater rptAlternateBikes;
        public int VersionId { get; set; }

        private int _topCount = 6;
        public int TopCount
        {
            get { return _topCount; }
            set { _topCount = value; }
        }

        public int? Deviation { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindAlternativeBikes();            
        }

        private void BindAlternativeBikes()
        {
            BindAlternativeBikesControl.VersionId = VersionId;
            BindAlternativeBikesControl.TopCpunt = TopCount;
            BindAlternativeBikesControl.Deviation = Deviation;
            BindAlternativeBikesControl.BindAlternativeBikes(rptAlternateBikes);
        }
    }
}