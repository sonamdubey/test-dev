/*******************************************************************************************************
THIS CLASS IS FOR SHOWING THE LIST OF ENTRIES MADE ACCORDING TO THE MAKE, YEAR, BODY STYLE AND SEGMENT
ALONG WITH THE NAME OF THE MAKE, ITS LOGO IS ALSO ASSOCIATED, THE PATH OF WHICH IS DEFINED IN A 
VARIABLE.
*******************************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Bikewale.Common;
using Bikewale.Controls;

namespace Bikewale.Used
{
	public class ViewAlbum : Page
	{
        public string inquiryId, profileNo, fileName = string.Empty, directoryPath;
       
        // variable used for paging photos
        public int pageNo = 1;
        public int scale = 8;

        protected Repeater rptPhotos;
		public DataSet dsData = new DataSet();
		
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
			if(Request["profileNo"] == null || Request.QueryString["profileNo"] == "")
			{
                UrlRewrite.Return404();
			}
			else
			{
				profileNo = Request.QueryString["profileNo"];
				inquiryId = CommonOpn.GetProfileNo(profileNo);
								
				//verify the id as passed in the url
                if (CommonOpn.CheckId(inquiryId) == false)
                {
                    UrlRewrite.Return404();
                }
			}
						
			if(Request["fileName"] != null || Request.QueryString["fileName"] != "")
			{
				fileName = Request.QueryString["fileName"];
			}						
			
			BindPhotos();
		
		} // Page_Load
		
		
		void BindPhotos()
		{
            throw new Exception("Method not used/commented");

            //string sql = "";
            //Database db = new Database();
            //DataSet ds = new DataSet();
						
            //Trace.Warn(sql);
            //try
            //{
            //    string whomBike = CommonOpn.CheckIsDealerFromProfileNo(profileNo) == true ? "1" : "0";	//1 for dealer Bike and 0 for customer Bike

            //    sql = " SELECT ImageUrlThumb, ImageUrlThumbSmall, ImageUrlFull, IsMain, DirectoryPath, HostUrl, OriginalImagePath FROM BikePhotos With(NoLock) "
            //        + " WHERE IsDealer = @whomBike AND IsActive = 1 AND IsApproved = 1 AND InquiryId = @inquiryId";
            //    Trace.Warn(sql);
            //    SqlParameter [] param ={new SqlParameter("@whomBike", whomBike), new SqlParameter("@inquiryId", inquiryId)};
            //    dsData = db.SelectAdaptQry(sql, param);

            //    DataRow[] drMain = dsData.Tables[0].Select("IsMain = 1");

            //    Trace.Warn("drMain : ", drMain.Length.ToString());

            //    if( fileName == "" || fileName == null)
            //    {									
            //        fileName = drMain[0]["ImageUrlFull"].ToString();                                      
            //    }

            //    directoryPath = drMain[0]["DirectoryPath"].ToString();

            //    rptPhotos.DataSource = dsData;
            //    rptPhotos.DataBind();										
            //}
            //catch (SqlException err)
            //{
            //    Trace.Warn(err.Message);
            //    ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    
            //}
            //catch(Exception err)
            //{
            //    Trace.Warn(err.Message);
            //    ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    
            //}
		}

		public string GetSelectedClass(string url)
		{
			if(url.ToLower() == fileName.ToLower())
				return "selected";
			else
				return "notSelected";
		}

        public string GetPageItemContainer()
        {
            string returnItem = "";

            if ((pageNo % scale) == 0)
            {
                pageNo = 1;
                returnItem = "</div><div>";
            }
            pageNo++;
            return returnItem;
        }
	} // class
} // namespace