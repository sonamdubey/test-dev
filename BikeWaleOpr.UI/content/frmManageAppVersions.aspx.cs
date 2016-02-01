using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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