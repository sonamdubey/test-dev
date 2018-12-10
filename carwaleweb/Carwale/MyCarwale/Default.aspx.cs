using System;
using System.Web.UI;
using Carwale.UI.ClientBL;
using System.Web;

namespace Carwale.UI.MyCarwale
{
    public class Default : Page
    {
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
           base.Load += new EventHandler(Page_Load);
        }
        
        void Page_Load(object Sender, EventArgs e)
        {
            if (!IsPostBack && DeviceDetectionManager.IsMobile(new HttpContextWrapper(HttpContext.Current)))
            {
                Response.Redirect("/used/mylistings/search/",false);
            }
        }
    }
} 