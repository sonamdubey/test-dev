/*
	This class will contain all the common function related to Sell Bike process
*/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using Bikewale.Common;

namespace Bikewale.Used
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 24/8/2012
    ///     Class contains functions related to classified bike inquiry photos
    /// </summary>
	public class ClassifiedInquiryPhotos
	{			
		//used for writing the debug messages
		private HttpContext objTrace = HttpContext.Current;
		
		// Constructor of the class
        /// <summary>
        /// Modified By : Sadhana on 9 Oct 2014
        /// Summary : removed condition for IsApprove flag in query
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="isDealer"></param>
        /// <param name="rptPhotos"></param>
		public void BindWithRepeater(string inquiryId, bool isDealer, Repeater rptPhotos, bool isAprooved)
		{
			string sql = "";

            HttpContext.Current.Trace.Warn("is dealer : ", isDealer.ToString());
            //sql = "SELECT Id, ImageUrlFull, ImageUrlThumb, ImageUrlThumbSmall, Description, IsMain, DirectoryPath, HostUrl FROM BikePhotos "
            //    + "WHERE IsActive = 1 AND InquiryId = @InquiryId AND IsDealer = @IsDealer ORDER BY IsMain DESC, Id ";

            //sql = " SELECT P.Id, P.ImageUrlFull, P.ImageUrlThumb, P.ImageUrlThumbSmall, P.Description, P.IsMain, P.DirectoryPath, P.HostUrl, P.StatusId "
            //    + " FROM BikePhotos AS P With(NoLock) "
            //    + " INNER JOIN ClassifiedIndividualSellInquiries AS SI With(NoLock) ON SI.ID = P.InquiryId "
            //    + " WHERE IsActive = 1 AND P.IsFake=0 AND InquiryId = @InquiryId AND IsDealer = @IsDealer"
            //    + " ORDER BY IsMain DESC, Id ";
			
			objTrace.Trace.Warn("sql : " + sql);
			
			try
			{
				//SqlParameter [] param ={new SqlParameter("@InquiryId", inquiryId), new SqlParameter("@IsDealer", isDealer)};
                SqlCommand cmd = new SqlCommand("GetListingPhotos");
                cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@InquiryId", SqlDbType.BigInt).Value = inquiryId;
				cmd.Parameters.Add("@IsDealer", SqlDbType.Bit).Value = isDealer;
                //cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = CurrentUser.Id;
                if (isAprooved)
                    cmd.Parameters.Add("@IsAprooved", SqlDbType.Bit).Value = isAprooved;
				
				Database db = new Database();
				DataSet ds = db.SelectAdaptQry(cmd);
				
				// Retrive front image from DataSet and assign then to respective properties
				DataRow[] row = ds.Tables[0].Select("IsMain = 1");
				
				if(row.Length > 0)
				{
					FrontImageMidThumb = row[0]["ImageUrlThumb"].ToString();
					FrontImageLarge = row[0]["ImageUrlFull"].ToString();
                    FrontImageDescription = row[0]["Description"].ToString();
                    DirectoryPath = row[0]["DirectoryPath"].ToString();
                    HostUrl = row[0]["HostUrl"].ToString();
                    OriginalImagePath = row[0]["OriginalImagePath"].ToString();
				}
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FrontImageMidThumb = ds.Tables[0].Rows[0]["ImageUrlThumb"].ToString();
                        FrontImageLarge = ds.Tables[0].Rows[0]["ImageUrlFull"].ToString();
                        FrontImageDescription = ds.Tables[0].Rows[0]["Description"].ToString();
                        DirectoryPath = ds.Tables[0].Rows[0]["DirectoryPath"].ToString();
                        HostUrl = ds.Tables[0].Rows[0]["HostUrl"].ToString();
                        OriginalImagePath = ds.Tables[0].Rows[0]["OriginalImagePath"].ToString();
                    }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    rptPhotos.DataSource = ds;
                    rptPhotos.DataBind();
                }				
				
				ClassifiedImageCount = ds.Tables[0].Rows.Count;
			}
			catch(Exception ex)
			{
				objTrace.Trace.Warn(ex.Message);				
				ErrorClass objErr = new ErrorClass(ex, objTrace.Request.ServerVariables["URL"]);
				objErr.SendMail();				
			}			
		}
		
		public static bool IsPhotoRequestDone(string sellInquiryId, string buyerId, bool isDealer)
		{
			bool isDone = false;
			
			string sql = "";				
			sql = "SELECT SellInquiryId FROM Classified_UploadPhotosRequest WHERE SellInquiryId = @SellInquiryId AND BuyerId = @BuyerId AND ConsumerType = @ConsumerType ";			
			
			string consumerType = isDealer ? "1" : "2";
			
			SqlDataReader dr = null;
			Database db = new Database();
			try
			{
				//SqlParameter [] param ={new SqlParameter("@InquiryId", inquiryId), new SqlParameter("@IsDealer", isDealer)};
				SqlCommand cmd =  new SqlCommand(sql);
				cmd.Parameters.Add("@SellInquiryId", SqlDbType.BigInt).Value = sellInquiryId;
				cmd.Parameters.Add("@BuyerId", SqlDbType.BigInt).Value = buyerId;
				cmd.Parameters.Add("@ConsumerType", SqlDbType.TinyInt).Value = consumerType;
				
				dr = db.SelectQry(cmd);
				
				if( dr.Read() )
				{
					isDone = true;
				}				
			}
			catch(Exception ex)
			{
				HttpContext.Current.Trace.Warn(ex.Message);				
				ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();				
			}
			finally
			{
				dr.Close();
				db.CloseConnection();
			}
			
			return isDone;			
		}
		
		public bool UploadPhotosRequest(string sellInquiryId, string buyerId, string consumerType, string buyerMessage)
		{
			bool isDone = false;
			
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			Database db = new Database();
						
			string conStr = db.GetConString();
			con = new SqlConnection( conStr );
			
			try
			{				
				cmd = new SqlCommand("Classified_UploadPhotosRequest_SP", con);
				cmd.CommandType = CommandType.StoredProcedure;
				
				prm = cmd.Parameters.Add("@SellInquiryId", SqlDbType.BigInt);
				prm.Value = sellInquiryId;
				
				prm = cmd.Parameters.Add("@BuyerId", SqlDbType.BigInt);
				prm.Value = buyerId;
				
				prm = cmd.Parameters.Add("@ConsumerType", SqlDbType.TinyInt);
				prm.Value = consumerType;	
				
				prm = cmd.Parameters.Add("@BuyerMessage", SqlDbType.VarChar, 200);
				prm.Value = buyerMessage;

                prm = cmd.Parameters.Add("@ClientIP", SqlDbType.VarChar, 40);
                prm.Value = CommonOpn.GetClientIP();

				con.Open();				
    			cmd.ExecuteNonQuery();
				
				isDone = true;              
			}			
			catch(Exception err)
			{				
				objTrace.Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
			finally
			{
				//close the connection	
			    if(con.State == ConnectionState.Open)
				{
					con.Close();
				}
			}
			
			return isDone;
		}				
		
		int _ClassifiedImageCount = 0;
		public int ClassifiedImageCount
		{
			get{ return _ClassifiedImageCount; }
			set{ _ClassifiedImageCount = value; }
		}
		
		string _FrontImageMidThumb = "";
		public string FrontImageMidThumb
		{
			get{ return _FrontImageMidThumb; }
			set{ _FrontImageMidThumb = value; }
		}
		
		string _FrontImageLarge = "";
		public string FrontImageLarge
		{
			get{ return _FrontImageLarge; }
			set{ _FrontImageLarge = value; }
		}
		
		string _FrontImageDescription = "";
		public string FrontImageDescription
		{
			get{ return _FrontImageDescription; }
			set{ _FrontImageDescription = value; }
		}

        string _DirectoryPath = "";
        public string DirectoryPath
        {
            get { return _DirectoryPath; }
            set { _DirectoryPath = value; }
        }

        string _HostUrl = "";
        public string HostUrl
        {
            get { return _HostUrl; }
            set { _HostUrl = value; }
        }

        public string OriginalImagePath { get; set; }
	}
}