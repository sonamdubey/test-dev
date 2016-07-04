/*
	This class will contain all the common function related to Sell Bike process
*/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Bikewale.Common;
using System.Collections.Specialized;
using Bikewale.Notifications.CoreDAL;
using System.Data.Common;

namespace Bikewale.Used
{
    public class ClassifiedBuyerDetails
    {
        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;

        string _BuyerName = "";
        public string BuyerName
        {
            get { return _BuyerName; }
            set { _BuyerName = value; }
        }

        string _BuyerMobile = "";
        public string BuyerMobile
        {
            get { return _BuyerMobile; }
            set { _BuyerMobile = value; }
        }

        string _BuyerEmail = "";
        public string BuyerEmail
        {
            get { return _BuyerEmail; }
            set { _BuyerEmail = value; }
        }

        string _BuyerId = "";
        public string BuyerId
        {
            get { return _BuyerId; }
            set { _BuyerId = value; }
        }

        public bool SetBuyerDetails(string buyerDetails)
        {
            bool isDone = false;

            try
            {
                // set buyer details to cookies
                HttpCookie cookie = new HttpCookie("TempCurrentUser");
                cookie.Value = buyerDetails;
                cookie.Expires = DateTime.Now.AddHours(24);
                HttpContext.Current.Response.Cookies.Add(cookie);

                isDone = true;
            }
            catch (Exception ex)
            {
                objTrace.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, objTrace.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return isDone;
        }

        public void GetBuyerDetails()
        {
            try
            {
                if (objTrace.Request.Cookies["TempCurrentUser"] != null && objTrace.Request.Cookies["TempCurrentUser"].Value.ToString() != "")
                {
                    string userData = objTrace.Request.Cookies["TempCurrentUser"].Value.ToString();

                    if (userData.Length > 0 && userData.IndexOf(':') > 0)
                    {
                        string[] details = userData.Split(':');

                        BuyerName = details[0];
                        BuyerMobile = details[1];
                        BuyerEmail = details[2] == "" ? CurrentUser.Email : details[2];
                        BuyerId = BikewaleSecurity.DecryptUserId(Convert.ToInt64(details[3]));
                    }
                }
            }
            catch (Exception ex)
            {
                objTrace.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, objTrace.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        public bool BookmarkThisBike(string customerId, string bikeProfileId)
        {
            throw new Exception("Method not used/commented");

            //if (customerId == "-1" || customerId == "") return false;

            //bool returnVal = false;

            //SqlConnection con;
            //SqlCommand cmd;
            //SqlParameter prm;
            //Database db = new Database();

            //string conStr = db.GetConString();

            //con = new SqlConnection(conStr);

            //try
            //{
            //    cmd = new SqlCommand("EntrySaveUsedBike", con);
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    prm = cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt);
            //    prm.Value = customerId;

            //    prm = cmd.Parameters.Add("@BikeProfileId", SqlDbType.VarChar, 50);
            //    prm.Value = bikeProfileId;
                													
            //    prm = cmd.Parameters.Add("@EntryDateTime", SqlDbType.DateTime);
            //    prm.Value = DateTime.Now;

            //    con.Open();
            //    //run the command
            // Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);
            //    cmd.ExecuteNonQuery();

            //    returnVal = true;

            //}
            //catch (Exception err)
            //{
            //    HttpContext.Current.Trace.Warn("Ajaxfunctions : EntrySaveUsedBike : " + err.Message);
            //    ErrorClass objErr = new ErrorClass(err, "Ajaxfunctions.EntrySaveUsedBike");
            //    objErr.SendMail();
            //    returnVal = false;
            //} // catch Exception
            //finally
            //{
            //    //close the connection	
            //    if (con.State == ConnectionState.Open)
            //    {
            //        con.Close();
            //    }
            //}

            //return returnVal;
        }

        public bool IsBuyerEligible(string buyerMobile)
        {
            bool status = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_restricbuyer_sp"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requestdate", DbType.DateTime, DateTime.Today));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 10, buyerMobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_status", DbType.Boolean, ParameterDirection.Output));

                Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);											
                    MySqlDatabase.ExecuteNonQuery(cmd);//run the command

                    status = Convert.ToBoolean(cmd.Parameters["par_status"].Value);
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Ajaxfunctions : EntrySaveUsedBike : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Ajaxfunctions.EntrySaveUsedBike");
                objErr.SendMail();
            } // catch Exception
            return status;
        }


        /**
         *  Summary : Fuction to set the buyer alerts & uses stored procedure.
         *  Auther  : Ashish G. Kamble created on 12/1/2012
         *  
         */
        public bool SetBuyerAlert(string email, string alertFrq, string url, string city, string budget, string year,
                                  string kms, string make, string model, string bodyStyle, string fuel,
                                  string engine, string transmission, string seller)
        {
            //if (Validations.IsValidEmail(email))
            //{
            //    return false;
            //}
            throw new Exception("Method not used/commented");

            //string cityId = string.Empty,
            //       cityDistance = string.Empty,
            //       budgetId = string.Empty,
            //       yearId = string.Empty,
            //       KmsId = string.Empty,
            //       makeId = string.Empty,
            //       modelId = string.Empty,
            //       fuelTypeId = string.Empty,
            //       bodyStyleId = string.Empty,
            //       transmissionId = string.Empty,
            //       sellerId = string.Empty;

            //bool status = false;

            //NameValueCollection nameValCol = new NameValueCollection();
            //nameValCol = HttpUtility.ParseQueryString(url);

            //cityId = nameValCol.Get("city");
            //cityDistance = nameValCol.Get("dist");
            //budgetId = nameValCol.Get("budget");
            //yearId = nameValCol.Get("year");
            //KmsId = nameValCol.Get("kms");
            //makeId = nameValCol.Get("make");
            //modelId = nameValCol.Get("model");
            //fuelTypeId = nameValCol.Get("fuel");
            //bodyStyleId = nameValCol.Get("bs");
            //transmissionId = nameValCol.Get("tm");
            //sellerId = nameValCol.Get("seller");

            ////if( cityId == string.Empty || cityDistance == string.Empty )
            ////{
            ////    return false;
            ////}

            //SqlCommand cmd = null;
            //Database db = null;
            //SqlConnection con = null;

            //try
            //{
            //    db = new Database();
            //    string conStr = db.GetConString();
            //    con = new SqlConnection(conStr);
            //    cmd = new SqlCommand();
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.CommandText = "UCAlert.SetUsedBikeCustomerSearchCriteria";
            //    cmd.Connection = con;

            //    cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = CurrentUser.Id;
            //    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = email;
            //    cmd.Parameters.Add("@CityId", SqlDbType.SmallInt, 50).Value = cityId == string.Empty ? Convert.DBNull : cityId;
            //    cmd.Parameters.Add("@City", SqlDbType.VarChar, 100).Value = city;
            //    cmd.Parameters.Add("@CityDistance", SqlDbType.SmallInt, 50).Value = cityDistance == string.Empty ? Convert.DBNull : cityDistance;
            //    cmd.Parameters.Add("@BudgetId", SqlDbType.VarChar, 50).Value = budgetId == string.Empty ? Convert.DBNull : budgetId;
            //    cmd.Parameters.Add("@Budget", SqlDbType.VarChar, 100).Value = budget == "" ? Convert.DBNull : budget;
            //    cmd.Parameters.Add("@YearId", SqlDbType.VarChar, 50).Value = yearId == string.Empty ? Convert.DBNull : yearId;
            //    cmd.Parameters.Add("@MakeYear", SqlDbType.VarChar, 100).Value = year == "" ? Convert.DBNull : year;
            //    cmd.Parameters.Add("@KmsId", SqlDbType.VarChar, 50).Value = KmsId == string.Empty ? Convert.DBNull : KmsId;
            //    cmd.Parameters.Add("@Kms", SqlDbType.VarChar, 100).Value = kms == "" ? Convert.DBNull : kms;
            //    cmd.Parameters.Add("@MakeId", SqlDbType.VarChar, 50).Value = makeId == string.Empty ? Convert.DBNull : makeId;
            //    cmd.Parameters.Add("@Make", SqlDbType.VarChar, 100).Value = make == "" ? Convert.DBNull : make;
            //    cmd.Parameters.Add("@ModelId", SqlDbType.VarChar, 50).Value = modelId == string.Empty ? Convert.DBNull : modelId;
            //    cmd.Parameters.Add("@Model", SqlDbType.VarChar, 100).Value = model == "" ? Convert.DBNull : model;
            //    cmd.Parameters.Add("@FuelTypeId", SqlDbType.VarChar, 50).Value = fuelTypeId == string.Empty ? Convert.DBNull : fuelTypeId;
            //    cmd.Parameters.Add("@FuelType", SqlDbType.VarChar, 100).Value = fuel == "" ? Convert.DBNull : fuel;
            //    cmd.Parameters.Add("@BodyStyleId", SqlDbType.VarChar, 50).Value = bodyStyleId == string.Empty ? Convert.DBNull : bodyStyleId;
            //    cmd.Parameters.Add("@BodyStyle", SqlDbType.VarChar, 100).Value = bodyStyle == "" ? Convert.DBNull : bodyStyle;
            //    cmd.Parameters.Add("@TransmissionId", SqlDbType.VarChar, 50).Value = transmissionId == string.Empty ? Convert.DBNull : transmissionId;
            //    cmd.Parameters.Add("@Transmission", SqlDbType.VarChar, 100).Value = transmission == string.Empty ? Convert.DBNull : transmission;
            //    cmd.Parameters.Add("@SellerId", SqlDbType.VarChar, 50).Value = sellerId == string.Empty ? Convert.DBNull : sellerId;
            //    cmd.Parameters.Add("@Seller", SqlDbType.VarChar, 100).Value = seller == string.Empty ? Convert.DBNull : seller;
            //    cmd.Parameters.Add("@AlertFrequency", SqlDbType.TinyInt).Value = alertFrq;
            //    cmd.Parameters.Add("@alertUrl", SqlDbType.VarChar, 8000).Value = url;
            //    cmd.Parameters.Add("@Status", SqlDbType.Bit).Direction = ParameterDirection.Output;
                Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);
            //    con.Open();
            //    cmd.ExecuteNonQuery();//run the command

            //    status = Convert.ToBoolean(cmd.Parameters["@Status"].Value);    // set status of database operation
            //}
            //catch (SqlException ex)
            //{
            //    HttpContext.Current.Trace.Warn(ex.Message);
            //    ErrorClass objEx = new ErrorClass(ex, "Ajaxfunctions.EntrySaveUsedBike");
            //}
            //catch (Exception err)
            //{
            //    HttpContext.Current.Trace.Warn("Ajaxfunctions : EntrySaveUsedBike : " + err.Message);
            //    ErrorClass objErr = new ErrorClass(err, "Ajaxfunctions.EntrySaveUsedBike");
            //    objErr.SendMail();
            //} // catch Exception
            //finally
            //{
            //    //close the connection	
            //    if (con.State == ConnectionState.Open)
            //    {
            //        con.Close();
            //    }

            //    db.CloseConnection();
            //}
            //return status;  // return status
        }   // End of SetBuyerAlert 

    }   // End of Class
}   // End of namespace Bikewale.Used