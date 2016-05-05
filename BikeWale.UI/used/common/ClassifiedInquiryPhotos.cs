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
using Bikewale.Notifications.CoreDAL;
using System.Data.Common;

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
        /// Modified By :   Sumit Kate on 23 Dec 2015
        /// Description :   Added Null Check for DataSet and DataTable and Check Inquiry Id is valid number
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="isDealer"></param>
        /// <param name="rptPhotos"></param>
        public void BindWithRepeater(string inquiryId, bool isDealer, Repeater rptPhotos, bool isAprooved)
        {
            string sql = "";

            HttpContext.Current.Trace.Warn("is dealer : ", isDealer.ToString());

            objTrace.Trace.Warn("sql : " + sql);

            try
            {
                //SqlParameter [] param ={new SqlParameter("@InquiryId", inquiryId), new SqlParameter("@IsDealer", isDealer)};
                if (!String.IsNullOrEmpty(inquiryId) && CommonOpn.CheckLongId(inquiryId))
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("getlistingphotos"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.Add("@InquiryId", SqlDbType.BigInt).Value = inquiryId;
                        //cmd.Parameters.Add("@IsDealer", SqlDbType.Bit).Value = isDealer;
                        ////cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = CurrentUser.Id;
                        //if (isAprooved)
                        //    cmd.Parameters.Add("@isaprooved", SqlDbType.Bit).Value = isAprooved;

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], inquiryId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_isdealer", DbParamTypeMapper.GetInstance[SqlDbType.Bit], isDealer));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], CurrentUser.Id));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_isaprooved", DbParamTypeMapper.GetInstance[SqlDbType.Bit], isAprooved));


                        using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                        {
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            {

                                // Retrive front image from DataSet and assign then to respective properties
                                DataRow[] row = ds.Tables[0].Select("IsMain = 1");

                                if (row.Length > 0)
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
                        }  
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

        public static bool IsPhotoRequestDone(string sellInquiryId, string buyerId, bool isDealer)
        {
            bool isDone = false;

            string sql = "";
            sql = "select sellinquiryid from classified_uploadphotosrequest where sellinquiryid = @par_sellinquiryid and buyerid = @par_buyerid and consumertype = @par_consumertype ";

            string consumerType = isDealer ? "1" : "2";
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    //SqlParameter [] param ={new SqlParameter("@InquiryId", inquiryId), new SqlParameter("@IsDealer", isDealer)};
                    //SqlCommand cmd = new SqlCommand(sql);
                    //cmd.Parameters.Add("@SellInquiryId", SqlDbType.BigInt).Value = sellInquiryId;
                    //cmd.Parameters.Add("@BuyerId", SqlDbType.BigInt).Value = buyerId;
                    //cmd.Parameters.Add("@ConsumerType", SqlDbType.TinyInt).Value = consumerType;

                    cmd.Parameters.Add(DbFactory.GetDbParam("@par_sellinquiryid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], sellInquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@par_buyerid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], buyerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@par_consumertype", DbParamTypeMapper.GetInstance[SqlDbType.TinyInt], consumerType));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr.Read())
                        {
                            isDone = true;
                        } 
                    } 
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return isDone;
        }

        public bool UploadPhotosRequest(string sellInquiryId, string buyerId, string consumerType, string buyerMessage)
        {
            bool isDone = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_uploadphotosrequest_sp"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //prm = cmd.Parameters.Add("@sellinquiryid", SqlDbType.BigInt);
                    //prm.Value = sellInquiryId;

                    //prm = cmd.Parameters.Add("@buyerid", SqlDbType.BigInt);
                    //prm.Value = buyerId;

                    //prm = cmd.Parameters.Add("@consumertype", SqlDbType.TinyInt);
                    //prm.Value = consumerType;

                    //prm = cmd.Parameters.Add("@buyermessage", SqlDbType.VarChar, 200);
                    //prm.Value = buyerMessage;

                    //prm = cmd.Parameters.Add("@clientip", SqlDbType.VarChar, 40);
                    //prm.Value = CommonOpn.GetClientIP();

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_sellinquiryid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], sellInquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_buyerid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], buyerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_consumertype", DbParamTypeMapper.GetInstance[SqlDbType.TinyInt], consumerType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_buyermessage", DbParamTypeMapper.GetInstance[SqlDbType.VarChar],200, buyerMessage));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbParamTypeMapper.GetInstance[SqlDbType.VarChar],40, CommonOpn.GetClientIP()));  

                    MySqlDatabase.ExecuteNonQuery(cmd);

                    isDone = true; 
                }
            }
            catch (Exception err)
            {
                objTrace.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception

            return isDone;
        }

        int _ClassifiedImageCount = 0;
        public int ClassifiedImageCount
        {
            get { return _ClassifiedImageCount; }
            set { _ClassifiedImageCount = value; }
        }

        string _FrontImageMidThumb = "";
        public string FrontImageMidThumb
        {
            get { return _FrontImageMidThumb; }
            set { _FrontImageMidThumb = value; }
        }

        string _FrontImageLarge = "";
        public string FrontImageLarge
        {
            get { return _FrontImageLarge; }
            set { _FrontImageLarge = value; }
        }

        string _FrontImageDescription = "";
        public string FrontImageDescription
        {
            get { return _FrontImageDescription; }
            set { _FrontImageDescription = value; }
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