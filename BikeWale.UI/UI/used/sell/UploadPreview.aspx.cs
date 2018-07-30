using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.Common;

namespace Bikewale.Used
{
	public class UploadPreview : Page
	{		
		protected Repeater rptImageList;
		
		public ClassifiedInquiryPhotos objPhotos;
		
		public string inquiryId = "-1";
		
		bool isDealer = false;
		
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

                bool isAprooved = true;
				objPhotos = new ClassifiedInquiryPhotos();
				objPhotos.BindWithRepeater(inquiryId, isDealer, rptImageList,isAprooved);
			}			
		}			
	}
}
