using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using TCClientInq.Proxy;

namespace BikeBookingInquiries
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Class to push the unpushed bike booking inquiry into autobiz.
    /// </summary>
    public class PushInquiryToAutoBiz
    {
        string strConn = ConfigurationManager.ConnectionStrings["bwconnectionstring"].ConnectionString;

        List<InquiryInfo> inquiryInfo = null;

        #region ProcessInquiries
        /// <summary>
        /// Function have business logic to process the unpushed autobiz inquiries.
        /// </summary>
        public void ProcessInquiries()
        {
            try
            {
                // Get inquiries from bikewale database which are not pushed to the autobiz for the one day.
                GetUnpushedABInquiries();

                // Iterate for each inquiry
                if (inquiryInfo != null)
                {
                    foreach (var inquiry in inquiryInfo)
                    {
                        // push inquiries to autobiz
                        PushInquiryInAB(inquiry.DealerId, inquiry.PQId, inquiry.CustomerName, inquiry.CustomerMobile, inquiry.CustomerEmail, inquiry.VersionId, inquiry.CityId);
                    };
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(ex.StackTrace);
            }           
        } 
        #endregion

        #region GetUnpushedABInquiries
        /// <summary>
        /// Function to get the all inquiries which are not pushed into autobiz. 
        /// Inquiries for one day will be retrieved.
        /// </summary>
        public void GetUnpushedABInquiries()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "GetUnpushedABBikeBookingInquiries";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        inquiryInfo = new List<InquiryInfo>();

                        conn.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                inquiryInfo.Add(new InquiryInfo()
                                {
                                    DealerId = dr["DealerId"].ToString(),
                                    PQId = dr["PQId"].ToString(),
                                    CustomerName = dr["CustomerName"].ToString(),
                                    CustomerEmail = dr["CustomerEmail"].ToString(),
                                    CustomerMobile = dr["CustomerMobile"].ToString(),
                                    VersionId = dr["BikeVersionId"].ToString(),
                                    CityId = dr["CityId"].ToString()
                                });
                            }
                        }

                        conn.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Logs.WriteErrorLog(ex.StackTrace);
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(ex.StackTrace);
            }

        }   // End of GetUnpushedABInquiries 
        #endregion

        #region PushInquiryInAB
        /// <summary>
        ///  Written By : Ashish G. Kamble
        /// Summary : Function to push the inquiry to the autobiz. Lead should be pushed only if mobile number is verified.
        /// </summary>
        /// <param name="branchId">Dealer id</param>
        /// <param name="pqId">Price quote Id</param>
        /// <param name="customerName">Name of the customer</param>
        /// <param name="customerMobile">mobile no of the customer</param>
        /// <param name="customerEmail">email id of the customer</param>
        /// <param name="versionId">bike version id</param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public bool PushInquiryInAB(string branchId, string pqId, string customerName, string customerMobile, string customerEmail, string versionId, string cityId)
        {
            bool isSuccess = false;
            string abInquiryId = string.Empty;

            try
            {
                string jsonInquiryDetails = "{\"CustomerName\":\"" + customerName + "\", \"CustomerMobile\":\"" + customerMobile + "\", \"CustomerEmail\":\"" + customerEmail + "\", \"VersionId\":\"" + versionId + "\", \"CityId\":\"" + cityId + "\", \"InquirySourceId\":\"39\", \"Eagerness\":\"1\",\"ApplicationId\":\"2\"}";

                TCApi_Inquiry objInquiry = new TCApi_Inquiry();
                abInquiryId = objInquiry.AddNewCarInquiry(branchId, jsonInquiryDetails);

                // update the bikewale tables with abinquiry id, keep log of the inquiries pushed through this application.
                if (!String.IsNullOrEmpty(abInquiryId))
                {                    
                    if (abInquiryId != "0" && abInquiryId != "-1")
                    {
                        // Successfully pushed
                        PushedToAB(pqId, abInquiryId, true);
                    }
                    else
                    {
                        // Some problem occured
                        PushedToAB(pqId, abInquiryId, false);
                    }
                }
                else
                {
                    // If inquiry not pushed to the autobiz (some error occured) keep log of the failed inquiries.
                    PushedToAB(pqId, abInquiryId, false);
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(ex.StackTrace);
            }
            return isSuccess;
        }   // End of PushInquiryInAB 
        #endregion

        #region PushedToAB
        /// <summary>
        /// Created By : Ashish G. Kamble on 14 May 2015
        /// Summary : To update the autobiz pushed/unpushed log.
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="abInquiryId"></param>
        /// <param name="isPushed"></param>
        public void PushedToAB(string pqId, string abInquiryId, bool isPushed)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "UpdateBikeBookingABPushStatus";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@pqId", SqlDbType.BigInt).Value = pqId;
                        cmd.Parameters.Add("@ABInquiryId", SqlDbType.BigInt).Value = String.IsNullOrEmpty(abInquiryId) ? Convert.DBNull : abInquiryId;
                        cmd.Parameters.Add("@IsPushed", SqlDbType.Bit).Value = isPushed;

                        conn.Open();

                        cmd.ExecuteNonQuery();

                        conn.Close();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Logs.WriteErrorLog(sqlEx.StackTrace);                
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(ex.StackTrace);                
            }
            
        }   // End of PushedToAB
        #endregion

    }   // class
}   // namespace
