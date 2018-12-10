using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MobileWeb.Common;
using MobileWeb.DataLayer;
using System.Text.RegularExpressions;
using Carwale.Interfaces;
using Carwale.Entity;
using Carwale.BL.Customers;

namespace MobileWeb.Users
{
	public class Logout : Page
	{
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{		
			CurrentUser.EndSession();



			HttpCookie rememberMe = Request.Cookies.Get("RememberMe");
			
			if ( rememberMe != null )
			{
                if (!string.IsNullOrEmpty(rememberMe.Value))
                {
                    string[] cred = rememberMe.Value.Split('~');
                    if (cred.Length == 6)
                    {
                        ICustomerBL<Customer, CustomerOnRegister> customerRepo = new CustomerActions<Customer, CustomerOnRegister>();
                        customerRepo.EndRememberMeSession(CurrentUser.Id, cred[3]);
                    }
                }

				rememberMe.Expires = DateTime.Now.AddDays(-1);
				Response.Cookies.Add( rememberMe );
			}

            Response.Redirect("~/m/");
		}
	}
}		