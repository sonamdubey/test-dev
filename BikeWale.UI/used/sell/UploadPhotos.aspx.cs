using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Bikewale.Common;

namespace Bikewale.Used
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 23/8/2012
    /// </summary>
	public class UploadPhotos : Page
	{		
		public string inquiryId = "-1";
		
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
			if ( !IsPostBack )
			{
				inquiryId = CookiesCustomers.SellInquiryId;
				
				if( ( CurrentUser.Id == "-1" && !CommonOpn.CheckId( CookiesCustomers.SellInquiryId )))
				{
					Response.Redirect("aboutbike.aspx");
				}		
			}			
		}			

	}   // End of class
}   // End of namespace
