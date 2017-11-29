using System;

namespace BikewaleOpr.content
{
    public class frmManageAppVersions : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        { }
    }
}