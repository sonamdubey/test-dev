using BikeWaleOpr.Common;
using BikeWaleOPR.DAL.CoreDAL;
using BikeWaleOPR.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getcustomerlistingdetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_startindex", DbParamTypeMapper.GetInstance[SqlDbType.Int], startIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_endindex", DbParamTypeMapper.GetInstance[SqlDbType.Int], endIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbParamTypeMapper.GetInstance[SqlDbType.Int], (!String.IsNullOrEmpty(inquiryId) && inquiryId != "0") ? Convert.ToUInt32(inquiryId.Substring(1, inquiryId.Length - 1)) : Convert.DBNull));
                    
                    ds = MySqlDatabase.SelectAdapterQuery(cmd);
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
            
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getlivelistings";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbParamTypeMapper.GetInstance[SqlDbType.Int], customerId));

                    
                    ds = MySqlDatabase.SelectAdapterQuery(cmd);
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
            
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getcustomerverifiedlistings";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbParamTypeMapper.GetInstance[SqlDbType.Int], customerId));

                    
                    ds = MySqlDatabase.SelectAdapterQuery(cmd);
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
            
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getcustomerfakelistings";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbParamTypeMapper.GetInstance[SqlDbType.Int], customerId));

                    
                    ds = MySqlDatabase.SelectAdapterQuery(cmd);
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
            
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getcustomerunverifiedlistings";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbParamTypeMapper.GetInstance[SqlDbType.Int], customerId));

                    
                    ds = MySqlDatabase.SelectAdapterQuery(cmd);
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
            
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getcustomersoldlistings";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbParamTypeMapper.GetInstance[SqlDbType.Int], customerId));

                    
                    ds = MySqlDatabase.SelectAdapterQuery(cmd);
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
            
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getcustomertotallistings";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbParamTypeMapper.GetInstance[SqlDbType.Int], customerId));

                    
                    ds = MySqlDatabase.SelectAdapterQuery(cmd);
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
            
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "listingphotos";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbParamTypeMapper.GetInstance[SqlDbType.Int], ProfileId));

                    
                    ds = MySqlDatabase.SelectAdapterQuery(cmd);
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
            
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "classified_inquiry_approve";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbParamTypeMapper.GetInstance[SqlDbType.Int], profileId));

                    
                    MySqlDatabase.UpdateQuery(cmd);
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
            
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "classified_inquiry_fake";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbParamTypeMapper.GetInstance[SqlDbType.Int], profileId));

                    
                    MySqlDatabase.UpdateQuery(cmd);
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
            
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "classified_bikephotos_markverified";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_photoidlist", DbParamTypeMapper.GetInstance[SqlDbType.VarChar],  photoIdList));

                    
                    MySqlDatabase.UpdateQuery(cmd);
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
            
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "classified_bikephotos_markfake";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_photoidlist", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], photoIdList));

                    
                    MySqlDatabase.UpdateQuery(cmd);
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
            
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "fakecustomer";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_custidlist", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], CustIdList));

                    
                    MySqlDatabase.UpdateQuery(cmd);
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