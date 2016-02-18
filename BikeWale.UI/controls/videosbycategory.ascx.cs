using System;
using System.Web.UI.WebControls;

namespace Bikewale.controls
{
    public partial class VideosByCategory : System.Web.UI.UserControl
    {
        protected Repeater rptCategoryVideos;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindVideosByCategory();
        }

        public void BindVideosByCategory()
        {

        }
    }
}