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
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.DAL.Forums;

namespace MobileWeb.Users
{
	public class CreateHandle : Page
	{
		protected LinkButton btnGetUserName;
		protected Label lblMessage;
		protected TextBox txtHandle;
		protected bool userNameAvailable = true;
	
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnGetUserName.Click += new EventHandler( btnGetUserName_Click );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			btnGetUserName.Attributes.Add("onclick","javascript:if(InputValid()==false)return false;");
		}	
		
		void btnGetUserName_Click( object Sender, EventArgs e )
		{
			ValidateHandleName();
			
			if (CommonOpn.HandleAvailable(txtHandle.Text.Trim()))
			{
				userNameAvailable = true;
				InsertHandle();
			}
			else
				userNameAvailable = false;
		}
		
		void ValidateHandleName()
		{
			if( ValidateHandle(txtHandle.Text.ToString()) == false )
			{
				lblMessage.Visible = true;
				return;
			}
			else
			{
				lblMessage.Visible = false;	
			}		
		}
				
		//Validation for Handle
		public bool ValidateHandle( string handle )
		{
			string reghandle = @"^[a-z_0-9_.]*$";
			bool val = true;

			if( handle == string.Empty )
				val = false;
			else if( handle.Substring(0,1) == "." )
				val = false;
			else if( handle.Length < 3 || handle.Length > 15 )
				val = false;
			if( !Regex.IsMatch( handle.ToLower(), reghandle ) )
				val = false;
			
			return val;
		}
		
		void InsertHandle()
		{
			string result = "";			
			try
			{
                UserDAL userRepo = new UserDAL();
                result = userRepo.InsertHandle(Convert.ToInt32(CurrentUser.Id), txtHandle.Text.Trim(), true).ToString();              								
				
				if(result.ToLower() != "false")
				{
					Response.Redirect(Request.QueryString["returnUrl"].ToString());
				}	
			}
			catch(Exception err)
			{
                if (err.Message != "Thread was being aborted.")
                {
                    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
			}
		}
	}
}		