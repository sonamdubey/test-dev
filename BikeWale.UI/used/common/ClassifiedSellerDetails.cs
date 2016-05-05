/*
	This class will contain all the common function related to Sell Car process
*/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Bikewale.Common;
using Bikewale.Notifications.CoreDAL;
using System.Data.Common;

namespace Bikewale.Used
{
	public class ClassifiedSellerDetails
	{			
		//used for writing the debug messages
		private HttpContext objTrace = HttpContext.Current;

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
			
			if( !isDealer )
			{
                //Modified By : Ashwini Todkar on 3 sep 2014
                //Retrieved Customer name, mobile and emailid from ClassifiedIndividualSellInquiries,so buyer get contact details of available with that inquiry
                sql = @" select si.customerid sellerid, si.customername as sellername, si.customeremail as selleremail,  
                     si.customermobile as contact, concat( c.city,', ',c.state )  as selleraddress, '' contactperson  
                     from classifiedindividualsellinquiries as si  
                     inner join customers as cu on cu.id = si.customerid 
                     inner join vwcity as c on c.cityid = si.cityid  
                    where si.id = @v_inquiryid ";
			}
			
			//cmd.Parameters.Add("@inquiryid", SqlDbType.BigInt).Value = inquiryId;
            

					
			try
			{
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@v_inquiryid", DbParamTypeMapper.GetInstance[SqlDbType.Int], inquiryId)); 

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr.Read())
                        {
                            SellerId = dr["SellerId"].ToString();
                            SellerName = dr["SellerName"].ToString();
                            SellerEmail = dr["SellerEmail"].ToString();
                            SellerContact = dr["Contact"].ToString();
                            SellerAddress = dr["SellerAddress"].ToString();
                            SellerContactPerson = dr["ContactPerson"].ToString();
                        } 
                    } 
                }
			}
			catch(Exception ex)
			{
				objTrace.Trace.Warn(ex.Message);				
				ErrorClass objErr = new ErrorClass(ex, objTrace.Request.ServerVariables["URL"]);
				objErr.SendMail();				
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