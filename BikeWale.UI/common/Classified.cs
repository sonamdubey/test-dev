using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Bikewale.Common
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 28/8/2012
    /// </summary>
    public class Classified
    {

        // this function tells if a particular customer has shown request for a particular Bike?
        public static bool HasShownInterestInUsedBike(bool isDealer, string bikeId, string customerId)
        {
            bool shownInterest = false;

            Database db = new Database();
            string sql = "";
            SqlDataReader dr = null;

            if (isDealer) // if it's a dealer bike
            {
                //sql = " SELECT ID AS RequestId "
                //    + " FROM UsedBikePurchaseInquiries "
                //    + " WHERE SellInquiryId=@InquiryId AND CustomerId=@CustomerId";
            }
            else // if it's an individual's Bike
            {
                sql = " SELECT ID AS RequestId "
                    + " FROM ClassifiedRequests With(NoLock) "
                    + " WHERE SellInquiryId=@InquiryId AND CustomerId=@CustomerId";
            }

            try
            {
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@InquiryId", SqlDbType.BigInt).Value = bikeId;
                cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = customerId;

                dr = db.SelectQry(cmd);

                if (dr.Read())
                {
                    shownInterest = true;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
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

            return shownInterest;
        }   // End of HasShownInterestInUsedBike function


    }   // End of class
}   // End of namespace