/*THIS CLASS HOLDS ALL TH EFUNCTION FOR BINDING GRID, FILLING DROPDOWN LIST AND OTHER SORTS OF
COMMON OPERATIONS.
*/

using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Mail;
using System.Data.Common;
using Bikewale.Notifications.CoreDAL;

namespace Bikewale.Common
{
	// This class retrieves customer info.
	// Customer Id must be provided in the constructor.
	public class SellInquiryDetails
	{
		private string inquiryId = "0";
		private string _customerId = "", _bikeName = "", _bikeMakeId, _bikeModelId,
				_bikeVersionId = "", _bikeMake, _bikeModel,
				_bikeVersion = "", _bikeRegNo = "", _entryDate = "",
				_price = "", _makeYear = "", _kilometers = "",
				_color = "", _comments = "", _cityId = "", _city = "",
				_bodyStyle = "", _segment = "", _emailEntered = "",
				_smallImgUrl = "", _bigImgUrl = "", _classifiedExpiryDate, _lastBidDate, 
				_registrationPlace="", _insurance="", _insuranceExpiry="", _owners="", 
				_tax="", _interiorColor="", _cityMileage="", _additionalFuel="", _bikeDriven="", 
				_accessories="", _warranties="", _modifications="", _batteryCondition="", _brakesCondition="", 
				_electricalsCondition="", _engineCondition="", _exteriorCondition="", 
				_seatsCondition="", _suspensionsCondition="", _tyresCondition="", 
				_overallCondition="", _accidental = "", _floodAffected = "", _packageType="", _viewCount = "", 
				_totalInquiries = "", _sellerEmail = "", _sellerName = "";
				
		private bool _forwardDealers = true, _listInClassifieds = true, _exists = false,
					_isVerified = false, _isFake = false, _isActive = false;
		
		public SellInquiryDetails()
		{
			// To Support some non-specific functions.
		}
		
		public SellInquiryDetails( string inquiryId )
		{
            this.InquiryId = inquiryId;            
		}

        public string GetInquiryDetails(string inquiryId)
        {

            string _status = string.Empty;

            string sql = @" select si.*, ci.cityid, ci.city, si.cityid as newcity, cu.email as emailentered, 
                si.statusid inqstatus, 
                ve.make, ve.model, ve.version, ve.makeid, ve.modelid,ve.versionid as bikeversionid, ve.smallpic, ve.largepic, 
                cu.name as sellername, cu.email as selleremail, cu.mobile sellermobile 
                from classifiedindividualsellinquiries si  
                left join vwmmv ve   on ve.versionid = si.bikeversionid 
                left join customers cu   on cu.id = si.customerid 
                left join vwcity as ci  on ci.cityid = si.cityid 
                where si.id = @inquiryid";



            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@inquiryid", DbType.Int64, inquiryId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr!=null && dr.Read())
                        {
                            this.Exists = true;

                            this.CustomerId = dr["CustomerId"].ToString();

                            this.BikeMake = dr["Make"].ToString();
                            this.BikeModel = dr["model"].ToString();
                            this.BikeVersion = dr["version"].ToString();
                            this.BikeName = this.BikeMake + " " + this.BikeModel + " " + this.BikeVersion;

                            this.SellerName = dr["SellerName"].ToString();
                            this.SellerEmail = dr["SellerEmail"].ToString();
                            this.SellerMobile = dr["SellerMobile"].ToString();

                            this.BikeMakeId = dr["MakeId"].ToString();
                            this.BikeModelId = dr["ModelId"].ToString();
                            this.BikeVersionId = dr["BikeVersionId"].ToString();
                            this.BikeRegNo = dr["BikeRegNo"].ToString();
                            this.EntryDate = dr["EntryDate"].ToString();
                            this.Price = dr["Price"].ToString();
                            this.MakeYear = Convert.ToDateTime(dr["MakeYear"].ToString()).ToString("MMM, yyyy");
                            this.Kilometers = dr["Kilometers"].ToString();
                            this.Color = dr["Color"].ToString();
                            this.Comments = dr["Comments"].ToString();

                            this.ViewCount = dr["ViewCount"].ToString();
                            bool _isapproved = false;
                            string isap = dr["IsApproved"].ToString();
                            if(!string.IsNullOrEmpty(isap) && bool.TryParse(isap,out _isapproved))
                            {
                                this.IsVerified = _isapproved;
                            }
                            else
                            {
                                this.IsVerified = false;
                            }
                            

                            this.CityId = dr["CityId"].ToString();
                            this.City = dr["City"].ToString();
                            //this.Segment = dr["Segment"].ToString();
                            //this.BodyStyle = dr["BodyStyle"].ToString();
                            this.EmailEntered = dr["EmailEntered"].ToString();

                            this.SmallImgUrl = dr["SmallPic"].ToString();
                            this.BigImgUrl = dr["LargePic"].ToString();
                            this.IsActive = dr["InqStatus"].ToString() == "1" ? true : false;

                            // check if details are available for this Bike.
                            if (dr["Id"].ToString().Length > 0)
                            {
                                this.RegistrationPlace = dr["RegistrationPlace"].ToString();
                                this.Insurance = dr["InsuranceType"].ToString();
                                this.InsuranceExpiry = dr["InsuranceExpiryDate"].ToString().Length > 0 ? Convert.ToDateTime(dr["InsuranceExpiryDate"].ToString()).ToString("dd-MMM-yy") : "";
                                this.Owners = dr["Owner"].ToString();
                                this.Tax = dr["LifetimeTax"].ToString();
                                this.BikeDriven = dr["Kilometers"].ToString();
                                this.Warranties = dr["Warranties"].ToString();
                                this.Modifications = dr["Modifications"].ToString();
                            }

                            dr.Close();
                        }
                    } 
                }
            }
            catch (SqlException err)
            {
                _status = "Sell Inquiry sql err : " + err.Message;
                HttpContext.Current.Trace.Warn("Sell Inquiry Control : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                throw;
            } // catch Exception
            catch (Exception err)
            {
                _status = "Sell Inquiry err : " + err.Message;
                HttpContext.Current.Trace.Warn("Sell Inquiry Control : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                throw;
            } // catch Exception

            return _status;
        }
		
		public string InquiryId
		{
			get { return inquiryId; }
			set { inquiryId = value; }
		}
		
		public string CustomerId
		{
			get { return _customerId; }
			set { _customerId = value; }
		}
		
		public string CityId
		{
			get { return _cityId; }
			set { _cityId = value; }
		}
		
		public string City
		{
			get { return _city; }
			set { _city = value; }
		}
		
		public string SellerName
		{
			get { return _sellerName; }
			set { _sellerName = value; }
		}
		
		public string SellerEmail
		{
			get { return _sellerEmail; }
			set { _sellerEmail = value; }
		}
		
		string _SellerMobile = "";
		public string SellerMobile
		{
			get { return _SellerMobile; }
			set { _SellerMobile = value; }
		}
		
		public string EmailEntered
		{
			get { return _emailEntered; }
			set { _emailEntered = value; }
		}
		
		public string SmallImgUrl
		{
			get { return _smallImgUrl; }
			set { _smallImgUrl = value; }
		}
		
		public string BigImgUrl
		{
			get { return _bigImgUrl; }
			set { _bigImgUrl = value; }
		}
		
		public string ClassifiedExpiryDate
		{
			get { return _classifiedExpiryDate; }
			set { _classifiedExpiryDate = value; }
		}
		
		public string LastBidDate
		{
			get { return _lastBidDate; }
			set { _lastBidDate = value; }
		}
				
		public string Segment
		{
			get { return _segment; }
			set { _segment = value; }
		}
		
		public string BodyStyle
		{
			get { return _bodyStyle; }
			set { _bodyStyle = value; }
		}
		
		public string BikeName
		{
			get { return _bikeName; }
			set { _bikeName = value; }
		}
		
		public string BikeMakeId
		{
			get { return _bikeMakeId; }
			set { _bikeMakeId = value; }
		}
		
		public string BikeModelId
		{
			get { return _bikeModelId; }
			set { _bikeModelId = value; }
		}
		
		public string BikeVersionId
		{
			get { return _bikeVersionId; }
			set { _bikeVersionId = value; }
		}
		
		public string BikeMake
		{
			get { return _bikeMake; }
			set { _bikeMake = value; }
		}
		
		public string BikeModel
		{
			get { return _bikeModel; }
			set { _bikeModel = value; }
		}
		
		public string BikeVersion
		{
			get { return _bikeVersion; }
			set { _bikeVersion = value; }
		}
		
		public string BikeRegNo
		{
			get { return _bikeRegNo; }
			set { _bikeRegNo = value; }
		}
		
		public string EntryDate
		{
			get { return _entryDate; }
			set { _entryDate = value; }
		}
		
		public string Price
		{
			get { return _price; }
			set { _price = value; }
		}
		
		public string MakeYear
		{
			get { return _makeYear; }
			set { _makeYear = value; }
		}
		
		public string Kilometers
		{
			get { return _kilometers; }
			set { _kilometers = value; }
		}
		
		public string Color
		{
			get { return _color; }
			set { _color = value; }
		}
		
		public string Comments
		{
			get { return _comments; }
			set { _comments = value; }
		}
		
		public bool ForwardDealers
		{
			get { return _forwardDealers; }
			set { _forwardDealers = value; }
		}
		
		public bool ListInClassifieds
		{
			get { return _listInClassifieds; }
			set { _listInClassifieds = value; }
		}
		
		public bool Exists
		{
			get { return _exists; }
			set { _exists = value; }
		}
		
		public bool IsActive
		{
			get { return _isActive; }
			set { _isActive = value; }
		}
		
		public bool IsVerified
		{
			get { return _isVerified; }
			set { _isVerified = value; }
		}
		
		public bool IsFake
		{
			get { return _isFake; }
			set { _isFake = value; }
		}
		
		public string Accidental
		{
			get { return _accidental; }
			set { _accidental = value; }
		}
		
		public string FloodAffected
		{
			get { return _floodAffected; }
			set { _floodAffected = value; }
		}
		
		public string RegistrationPlace
		{
			get { return _registrationPlace; }
			set { _registrationPlace = value; }
		}
		
		public string Insurance
		{
			get { return _insurance; }
			set { _insurance = value; }
		}
		
		public string InsuranceExpiry
		{
			get { return _insuranceExpiry; }
			set { _insuranceExpiry = value; }
		}
		
		public string Owners
		{
			get { return _owners; }
			set { _owners = value; }
		}
		
		public string Tax
		{
			get { return _tax; }
			set { _tax = value; }
		}
		
		public string InteriorColor
		{
			get { return _interiorColor; }
			set { _interiorColor = value; }
		}

		public string CityMileage
		{
			get { return _cityMileage; }
			set { _cityMileage = value; }
		}
		
		public string AdditionalFuel
		{
			get { return _additionalFuel; }
			set { _additionalFuel = value; }
		}
		
		public string BikeDriven
		{
			get { return _bikeDriven; }
			set { _bikeDriven = value; }
		}
		
		public string Accessories
		{
			get { return _accessories; }
			set { _accessories = value; }
		}
		
		public string Warranties
		{
			get { return _warranties; }
			set { _warranties = value; }
		}
		
		public string Modifications
		{
			get { return _modifications; }
			set { _modifications = value; }
		}
		
		public string BatteryCondition
		{
			get { return _batteryCondition; }
			set { _batteryCondition = value; }
		}
		
		public string BrakesCondition
		{
			get { return _brakesCondition; }
			set { _brakesCondition = value; }
		}
		
		public string ElectricalsCondition
		{
			get { return _electricalsCondition; }
			set { _electricalsCondition = value; }
		}
		
		public string EngineCondition
		{
			get { return _engineCondition; }
			set { _engineCondition = value; }
		}
		
		public string ExteriorCondition
		{
			get { return _exteriorCondition; }
			set { _exteriorCondition = value; }
		}
		
		public string SeatsCondition
		{
			get { return _seatsCondition; }
			set { _seatsCondition = value; }
		}
		
		public string SuspensionsCondition
		{
			get { return _suspensionsCondition; }
			set { _suspensionsCondition = value; }
		}
		
		public string TyresCondition
		{
			get { return _tyresCondition; }
			set { _tyresCondition = value; }
		}
		
		public string OverallCondition
		{
			get { return _overallCondition; }
			set { _overallCondition = value; }
		}
		
		public string PackageType
		{
			get { return _packageType; }
			set { _packageType = value; }
		}
		
		public string ViewCount
		{
			get { return _viewCount; }
			set { _viewCount = value; }
		}
		
		public string TotalInquiries
		{
			get { return _totalInquiries; }
			set { _totalInquiries = value; }
		}
		
		
		public string GetAveragePrice( string monthMake, string yearMake, string bikeVersionId )
		{
            throw new Exception("Method not used/commented");

            //string averagePrice = "";
			
            //SqlDataReader dr = null;
            //Database db = new Database();
            //string sql;
			
            //sql = " SELECT "
            //    + " ( SELECT AVG(Price) FROM SellInquiries With(NoLock) WHERE BikeVersionId=@bikeVersionId AND YEAR(MakeYear)=@yearMake ) YearAverage, "
            //    + " ( SELECT COUNT(Price) FROM SellInquiries With(NoLock) WHERE BikeVersionId=@bikeVersionId AND YEAR(MakeYear)=@yearMake ) YearCount, "
            //    + " ( SELECT AVG(Price) FROM SellInquiries With(NoLock) WHERE BikeVersionId=@bikeVersionId AND YEAR(MakeYear)=@yearMake AND MONTH(MakeYear)=@monthMake ) MonthAverage, "
            //    + " ( SELECT COUNT(Price) FROM SellInquiries With(NoLock) WHERE BikeVersionId=@bikeVersionId AND YEAR(MakeYear)=@yearMake AND MONTH(MakeYear)=@monthMake ) MonthCount ";
				
            //HttpContext.Current.Trace.Warn( "Sell Inquiry Control : " + sql );	
            //SqlParameter [] param ={new SqlParameter("@bikeVersionId", bikeVersionId), new SqlParameter("@yearMake", yearMake), new SqlParameter("@monthMake", monthMake)};
			
            //try
            //{
            //    dr = db.SelectQry( sql, param );
				
            //    if ( dr.Read() )
            //    {
            //        string yPrice = dr["YearAverage"].ToString();
					
            //        if ( yPrice.IndexOf(".") > -1 )
            //            yPrice = yPrice.Substring( 0, yPrice.IndexOf(".") );
					
            //        string mPrice = dr["MonthAverage"].ToString();
					
            //        if ( mPrice.IndexOf(".") > -1 )
            //            mPrice = mPrice.Substring( 0, mPrice.IndexOf(".") );
						
            //        averagePrice = "<br>Average Prices:<br>Year: <b>Rs." + yPrice + "/-</b>,  "
            //                        + dr["YearCount"].ToString() + " bikes. <br>"
            //                        + "Month-Year: <b>Rs." + mPrice + "/-</b>, "
            //                        + dr["MonthCount"].ToString() + " bikes. ";
            //    }				
            //}
            //catch( Exception err )
            //{
            //    HttpContext.Current.Trace.Warn( "Sell Inquiry Average Price : " + err.Message );	
            //    ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if (dr != null)
            //    {
            //        dr.Close();
            //    }
            //    db.CloseConnection();
            //}
			
            //return averagePrice;
		}
		
		public void SendConfirmations(string inquiryId)
		{
			string customerName = "";
			string mobile = "";
			string eMail = "";
			string bikeName = "";
			string profileId = "S" + inquiryId;
			string bikeVersionId = "";
			
			//Fetch Inquiry related details
			GetSellInquiryDetails(inquiryId, out customerName, out mobile, out eMail, out bikeName, out bikeVersionId);
			
			//Send Mail
			Mails.MailToPaidCustomer(customerName, mobile, eMail, profileId, bikeVersionId);
			
			//Send SMS
			SMSToPaidCustomer(mobile, bikeName);
		}
		
		void SMSToPaidCustomer(string mobile, string bikeName)
		{
			if(mobile != "" && bikeName != "")
			{
				SMSCommon sc = new SMSCommon();
				string url = HttpContext.Current.Request.ServerVariables["URL"];
				string message = "Your ad for " + bikeName + " is now live. You will receive enquiries via SMS and email. Thank you for choosing BikeWale.";
				
				sc.ProcessSMS(mobile, message, EnumSMSServiceType.CustomSMS, url);
			}
		}
		
		void GetSellInquiryDetails(string inquiryId, out string customerName, out string mobile, out string eMail, out string bikeName, out string versionId)
		{
            throw new Exception("Method not used/commented");

            //customerName = "";
            //mobile = "";
            //eMail = "";
            //bikeName = "";
            //versionId = "";
			
            //string sql = "";
            //SqlDataReader dr = null;
            //Database db = new Database();
			
            //sql = " SELECT (M.Make + ' ' + M.Model ) AS BikeName, "
            //    + " C.Email, C.Mobile, C.Name, CSI.BikeVersionId "
            //    + " FROM ClassifiedIndividualSellInquiries AS CSI With(NoLock) "
            //    + " INNER JOIN vwMMV AS M With(NoLock) ON M.VersionId = CSI.BikeVersionId "
            //    + " INNER JOIN Customers AS C With(NoLock) ON C.Id = CSI.CustomerId "
            //    + " WHERE CSI.ID = 22" + inquiryId + "";

            //try
            //{
            //    dr = db.SelectQry(sql);
            //    if (dr.Read())
            //    {
            //        bikeName = dr["BikeName"].ToString();
            //        customerName = dr["Name"].ToString();
            //        mobile = dr["Mobile"].ToString();
            //        eMail = dr["email"].ToString();
            //        versionId = dr["BikeVersionId"].ToString();
            //    }                               
            //}
            //catch (Exception err)
            //{
            //    HttpContext.Current.Trace.Warn(err.Message);
            //    ErrorClass objErr = new ErrorClass(err, "SellInquiryDetails.GetSellInquiryDetails()");
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    if (dr != null)
            //    {
            //        dr.Close();
            //    }
            //    db.CloseConnection();
            //}
		}


        #region GetInquiryDetailsByProfileId
        /// <summary>
        /// Written By : Ashwini Todkar on 30 Oct 2013
        /// Summary : Getting used bike details  by profileId
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns>datatable containing city, make and model name</returns>
        public DataTable GetInquiryDetailsByProfileId(string profileId)
        {
            DataTable dt = null;
           
            try
            {
                if (Validations.IsValidProfileId(profileId))
                {

                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "getinquirydetailsbyprofileid";

                        //cmd.Parameters.Add("@profileId", SqlDbType.VarChar, 50).Value = profileId;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_profileid", DbType.String,50, profileId)); 

                        using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                        {
                            if (ds != null && ds.Tables[0].Rows.Count > 0)
                            {
                                dt = ds.Tables[0];
                            }
                        }
                        
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return dt;
        } 
        #endregion
     
       
	}   // class
	
}//namespace
