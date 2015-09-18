using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

/// <summary>
/// Created By Sanjay Soni ON 1/10/2014
/// </summary>
namespace BikeWaleOpr.Classified
{
    public class ClassifiedCommon
    {
        #region CustomerListingDetail
        /// <summary>
        /// Created By : Sanjay Soni ON 30/9/2014
        /// Description : To Retrieve All Listings from start Index to end Index
        /// </summary>
        public DataSet CustomerListingDetail(int startIndex,int endIndex,string inquiryId = "")
        {
            Database db = null;
            DataSet ds = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetCustomerListingDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@StartIndex", SqlDbType.Int).Value = startIndex;
                    cmd.Parameters.Add("@EndIndex", SqlDbType.Int).Value = endIndex;
                    if (!String.IsNullOrEmpty(inquiryId))
                    {
                        cmd.Parameters.Add("@InquiryId", SqlDbType.Int).Value = Convert.ToUInt32(inquiryId.Substring(1, inquiryId.Length - 1));
                    }
                    db = new Database();
                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("CustomerListingDetail  sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("CustomerListingDetail ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }   //End of BindRepeater
        #endregion

        #region CustomerLiveListings
        /// <summary>
        /// Craeted By : Sanjay Soni on 3rd Oct 2014
        /// Description : To Retrieve All Live Listings for Particular Customer
        /// </summary>
        /// <param name="customerId"></param>
        public DataSet CustomerLiveListings(int customerId)
        {
            Database db = null;
            DataSet ds = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetLiveListings";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;

                    db = new Database();
                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("CustomerLiveListings  sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("CustomerLiveListings ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }
        #endregion

        #region CustomerPendingListings
        /// <summary>
        /// Craeted By : Sanjay Soni on 3rd Oct 2014
        /// Description : To Retrieve All Pending Listings for Particular Customer
        /// </summary>
        /// <param name="customerId"></param>
        public DataSet CustomerPendingListings(int customerId)
        {
            Database db = null;
            DataSet ds = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetCustomerVerifiedListings";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;

                    db = new Database();
                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("CustomerPendingListings  sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("CustomerPendingListings ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }
        #endregion

        #region CustomerFakeListings
        /// <summary>
        /// Craeted By : Sanjay Soni on 3rd Oct 2014
        /// Description : To Retrieve All Fake Listings for Particular Customer
        /// </summary>
        /// <param name="customerId"></param>
        public DataSet CustomerFakeListings(int customerId)
        {
            Database db = null;
            DataSet ds = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetCustomerFakeListings";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;

                    db = new Database();
                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("CustomerFakeListings  sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("CustomerFakeListings ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }
        #endregion

        #region CustomerUnVerifiedListings
        /// <summary>
        /// Craeted By : Sanjay Soni on 3rd Oct 2014
        /// Description : To Retrieve All Pending Listings for Particular Customer
        /// </summary>
        /// <param name="customerId"></param>
        public DataSet CustomerUnVerifiedListings(int customerId)
        {
            Database db = null;
            DataSet ds = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetCustomerUnVerifiedListings";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;

                    db = new Database();
                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("CustomerUnVerifiedListings  sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("CustomerUnVerifiedListings ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }
#endregion

        #region CustomerSoldListings
        /// <summary>
        /// Craeted By : Sanjay Soni on 3rd Oct 2014
        /// Description : To Retrieve All Sold Listings for Particular Customer
        /// </summary>
        /// <param name="customerId"></param>
        public DataSet CustomerSoldListings(int customerId)
        {
            Database db = null;
            DataSet ds = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetCustomerSoldListings";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;

                    db = new Database();
                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("CustomerSoldListings  sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("CustomerSoldListings ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }
        #endregion

        #region CustomerTotalListings
        /// <summary>
        /// Craeted By : Sanjay Soni on 3rd Oct 2014
        /// Description : To Retrieve All Listings for Particular Customer
        /// </summary>
        /// <param name="customerId"></param>
        public DataSet CustomerTotalListings(int customerId)
        {
            Database db = null;
            DataSet ds = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetCustomerTotalListings";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;

                    db = new Database();
                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("CustomerTotalListings  sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("CustomerTotalListings ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }
        #endregion

        #region CustomerTotalListingPhotos
        /// <summary>
        /// Craeted By : Sanjay Soni on 3rd Oct 2014
        /// Description : To Retrieve All Photos for Particular Customer
        /// </summary>
        /// <param name="profileId"></param>
        public DataSet CustomerTotalListingPhotos(int ProfileId)
        {
            Database db = null;
            DataSet ds = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "listingPhotos";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@InquiryId", SqlDbType.Int).Value = ProfileId;

                    db = new Database();
                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("CustomerTotalListingPhotos  sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("CustomerTotalListingPhotos ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }
        #endregion

        #region ApproveListing
        /// <summary>
        /// Craeted By : Sanjay Soni on 3rd Oct 2014
        /// Description : To Approve Inquiry Listing
        /// </summary>
        /// <param name="profileId"></param>
        public bool ApproveListing(int profileId)
        {
            bool isSuccess = false;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "Classified_Inquiry_Approve";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@inquiryId", SqlDbType.Int).Value = profileId;

                    db = new Database();
                    db.UpdateQry(cmd);
                    isSuccess = true;
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("ApproveListing  sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("ApproveListing ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            return isSuccess;
        }
        #endregion

        #region DiscardListing
        /// <summary>
        /// Craeted By : Sanjay Soni on 3rd Oct 2014
        /// Description : To Discard Fake Inquiry Listing
        /// </summary>
        /// <param name="profileId"></param>
        public bool DiscardListing(int profileId)
        {
            bool isSuccess = false;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "Classified_Inquiry_Fake";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@inquiryId", SqlDbType.Int).Value = profileId;

                    db = new Database();
                    db.UpdateQry(cmd);
                    isSuccess = true;
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("DiscardListing  sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("DiscardListing ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            return isSuccess;
        }
        #endregion

        #region ApproveSelectedPhotos
        /// <summary>
        /// Craeted By : Sanjay Soni on 3rd Oct 2014
        /// Description : To Approve Selected Photos
        /// </summary>
        /// <param name="photoIdList"></param>
        public void ApproveSelectedPhotos(string photoIdList)
        {
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "classified_BikePhotos_MarkVerified";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@photoIdList", SqlDbType.VarChar,-1).Value = photoIdList;

                    db = new Database();
                    db.UpdateQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("getCustomerApproveSelectedPhotos  sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("getCustomerApproveSelectedPhotos ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
#endregion

        #region DiscardSelectedPhotos
        /// <summary>
        /// Craeted By : Sanjay Soni on 3rd Oct 2014
        /// Description : To Discard Fake Photos
        /// </summary>
        /// <param name="photoIdList"></param>
        public void DiscardSelectedPhotos(string photoIdList)
        {
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "classified_BikePhotos_MarkFake";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@photoIdList", SqlDbType.VarChar,-1).Value = photoIdList;

                    db = new Database();
                    db.UpdateQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("DiscardSelectedPhotos  sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("DiscardSelectedPhotos ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        #endregion

        #region DiscardCustomers
        /// <summary>
        /// Craeted By : Sanjay Soni on 3rd Oct 2014
        /// Description : To Mark Fake Customers
        /// </summary>
        /// <param name="CustIdList"></param>
        public void DiscardCustomers(string CustIdList)
        {
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "FakeCustomer";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CustIdList", SqlDbType.VarChar, -1).Value = CustIdList;

                    db = new Database();
                    db.UpdateQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("DiscardCustomers  sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("DiscardCustomers ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   //End of DiscardCustomers
        #endregion
    } // END CLASS
} // END NAMESPACE