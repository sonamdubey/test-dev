using System;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using BikeWaleOpr.Common;
using FreeTextBoxControls;
using Ajax;

namespace BikeWaleOpr.Content
{
    public class CompareFeaturedBike : Page
    {
        protected DropDownList ddlMake, ddlModel, ddlVersion, ddlMakeFeaturedBike, ddlModelFeaturedBike;
        protected TextBox tbxText, txtVersionsHidden;

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
            //CommonOpn op = new CommonOpn();

            //if (HttpContext.Current.User.Identity.IsAuthenticated != true)
            //    Response.Redirect("/users/Login.aspx?ReturnUrl=/Contents/CompareFeaturedCar.aspx");

            //if (Request.Cookies["Customer"] == null)
            //    Response.Redirect("/Users/Login.aspx?ReturnUrl=/Contents/CompareFeaturedCar.aspx");

            //int pageId = 38;
            //if (!op.verifyPrivilege(pageId))
            //    Response.Redirect("/NotAuthorized.aspx");

            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
            if (!IsPostBack)
            {
                BindControls.BindAllMakes(ddlMake);
                ddlModel.Items.Insert(0, new ListItem("--Select--", "-1"));
                ddlVersion.Items.Insert(0, new ListItem("--Select--", "-1"));

                BindControls.BindAllMakes(ddlMakeFeaturedBike);
                ddlModelFeaturedBike.Items.Insert(0, new ListItem("--Select--", "-1"));

            }
            else
            {
                //AjaxFunctions aj = new AjaxFunctions();
                //aj.UpdateContents(ddlModels, ModelContents, SelectedModel);
            }
        }

    }
}