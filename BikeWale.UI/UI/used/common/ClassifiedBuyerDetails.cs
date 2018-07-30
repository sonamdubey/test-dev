using Bikewale.Common;
using MySql.CoreDAL;
/*
	This class will contain all the common function related to Sell Bike process
*/
using System;
using System.Data;
using System.Data.Common;
using System.Web;

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
                ErrorClass.LogError(ex, objTrace.Request.ServerVariables["URL"]);
                
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
                ErrorClass.LogError(ex, objTrace.Request.ServerVariables["URL"]);
                
            }
        }

        public bool BookmarkThisBike(string customerId, string bikeProfileId)
        {
            throw new Exception("Method not used/commented");
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

                    //Bikewale.Notifications.// LogLiveSps.LogSpInGrayLog(cmd);											
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);//run the command

                    status = Convert.ToBoolean(cmd.Parameters["par_status"].Value);
                }
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Ajaxfunctions : EntrySaveUsedBike : " + err.Message);
                ErrorClass.LogError(err, "Ajaxfunctions.EntrySaveUsedBike");
                
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
          
            throw new Exception("Method not used/commented");

        }   // End of SetBuyerAlert 

    }   // End of Class
}   // End of namespace Bikewale.Used