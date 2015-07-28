/*
	This class will contain all the common function related to Sell Car process
*/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Bikewale.Common;

namespace Bikewale.Used
{
	public class ClassifiedSellerDetails
	{			
		//used for writing the debug messages
		private HttpContext objTrace = HttpContext.Current;
		
        //public DataSet GetSellerDetailsDataSet(string inquiryId, bool isDealer)
        //{  
        //    string sql = "";
        //    DataSet ds = null;

        //    if (isDealer)
        //    {
        //        //sql = " Select D.Organization AS SellerName, D.EmailId AS SellerEmail, "
        //        //    + " (D.MobileNo + CASE WHEN D.PhoneNo IS NOT NULL AND D.PhoneNo <> '' THEN ', ' + D.PhoneNo ELSE NULL END) AS Contact, "
        //        //    + " (D.Address1 +', '+ D.Address2) AS SellerAddress, ContactPerson "

        //        //    + " From SellInquiries AS SI, Dealers AS D, Cities AS Ct, States AS St "
        //        //    + " WHERE SI.DealerId = D.ID AND D.CityId = Ct.ID AND Ct.StateId = St.ID AND SI.ID = @InquiryId ";
        //    }
        //    else
        //    {
        //        sql = " SELECT CU.Name AS SellerName, CU.Email AS SellerEmail, "
        //            + " CU.Mobile AS Contact, ( C.City +', '+ C.State )  AS SellerAddress, '' ContactPerson "
        //            + " FROM ClassifiedIndividualSellInquiries AS SI "
        //            + " INNER JOIN Customers AS CU ON CU.Id = SI.CustomerId "
        //            + " INNER JOIN vwCity AS C ON C.CityId = SI.CityId "
        //            + " WHERE SI.ID = @InquiryId ";
        //    }

        //    SqlCommand cmd = new SqlCommand(sql);
        //    cmd.Parameters.Add("@InquiryId", SqlDbType.BigInt).Value = inquiryId;

        //    try
        //    {
        //        Database db = new Database();
        //        ds = db.SelectAdaptQry(cmd);
        //    }
        //    catch (Exception ex)
        //    {
        //        objTrace.Trace.Warn(ex.Message);
        //        ErrorClass objErr = new ErrorClass(ex, objTrace.Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
            
        //    return ds;
        //}
		
		public bool IsClassifiedIntruder(string inquiryId, bool isDealer)
		{
			bool isIntruder = false;
			GetSellerDetails(inquiryId, isDealer);
			
			if( SellerId != CurrentUser.Id )
				isIntruder = true;
				
			return isIntruder;
		}
		
		public void GetSellerDetails(string inquiryId, bool isDealer)
		{
			string sql = "";			
			
			if( isDealer )
			{
                //sql = " Select DealerId SellerId, D.Organization AS SellerName, D.EmailId AS SellerEmail, "
                //    + " (D.MobileNo + CASE WHEN D.PhoneNo IS NOT NULL AND D.PhoneNo <> '' THEN ', ' + D.PhoneNo ELSE NULL END) AS Contact, "
                //    + " (D.Address1 +', '+ D.Address2) AS SellerAddress, ContactPerson "
					 
                //    + " From SellInquiries AS SI, Dealers AS D, Cities AS Ct, States AS St "
                //    + " WHERE SI.DealerId = D.ID AND D.CityId = Ct.ID AND Ct.StateId = St.ID AND SI.ID = @InquiryId ";
			}
			else
			{
                //Modified By : Ashwini Todkar on 3 sep 2014
                //Retrieved Customer name, mobile and emailid from ClassifiedIndividualSellInquiries,so buyer get contact details of available with that inquiry
                sql = " SELECT SI.CustomerId SellerId, SI.CustomerName AS SellerName, SI.CustomerEmail AS SellerEmail, "
                    + " SI.CustomerMobile AS Contact, ( C.City +', '+ C.State )  AS SellerAddress, '' ContactPerson "
                    + " FROM ClassifiedIndividualSellInquiries AS SI "
                    + " INNER JOIN Customers AS CU ON CU.Id = SI.CustomerId "
                    + " INNER JOIN vwCity AS C ON C.CityId = SI.CityId "
                    + " WHERE SI.ID = @InquiryId ";
			}
			
			SqlCommand cmd =  new SqlCommand(sql);
			cmd.Parameters.Add("@InquiryId", SqlDbType.BigInt).Value = inquiryId;
			
			SqlDataReader dr = null;
			Database db = new Database();
					
			try
			{
				dr = db.SelectQry(cmd);
				
				if( dr.Read() )
				{
					SellerId		= dr["SellerId"].ToString();
					SellerName		= dr["SellerName"].ToString();
					SellerEmail		= dr["SellerEmail"].ToString();
					SellerContact	= dr["Contact"].ToString();
					SellerAddress	= dr["SellerAddress"].ToString();
                    SellerContactPerson = dr["ContactPerson"].ToString();
				}
			}
			catch(Exception ex)
			{
				objTrace.Trace.Warn(ex.Message);				
				ErrorClass objErr = new ErrorClass(ex, objTrace.Request.ServerVariables["URL"]);
				objErr.SendMail();				
			}
			finally
			{
                if (dr != null)
                {
                    dr.Close();
                }
				db.CloseConnection();
			}			
		}

        string _SellerId = string.Empty;
		public string SellerId
		{
			get{ return _SellerId; }
			set{ _SellerId = value; }
		}

        string _SellerName = string.Empty;
		public string SellerName
		{
			get{ return _SellerName; }
			set{ _SellerName = value; }
		}

        string _SellerEmail = string.Empty;
		public string SellerEmail
		{
			get{ return _SellerEmail; }
			set{ _SellerEmail = value; }
		}

        string _SellerContact = string.Empty;
		public string SellerContact
		{
			get{ return _SellerContact; }
			set{ _SellerContact = value; }
		}

        string _SellerAddress = string.Empty;
		public string SellerAddress
		{
			get{ return _SellerAddress; }
			set{ _SellerAddress = value; }
		}

        string _SellerContactPerson = string.Empty;        
        public string SellerContactPerson
        {
            get { return _SellerContactPerson; }
            set { _SellerContactPerson = value; }
        }
	}
}