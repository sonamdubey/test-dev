using Bikewale.Notifications;
using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace NoPhotoListingsCommunication
{

    /// <summary>
    /// Created By:-Subodh Jain 24 Nov 2016
    /// summary:- For photo upload sms and mail
    /// </summary>
    public class NoPhotoSMSDAL
    {
        /// <summary>
        /// Created By:-Subodh Jain 24 Nov 2016
        /// summary:- For photo upload sms and mail
        /// </summary>
        public NoPhotoUserListEntity SendSMSNoPhoto()
        {
            Logs.WriteInfoLog("Started  NoPhotoSMSDAL");
            NoPhotoUserListEntity objNoPhotoList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_nophotolistings"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            objNoPhotoList = new NoPhotoUserListEntity();
                            objNoPhotoList.objTwoDaySMSList = new List<NoPhotoSMSEntity>();
                            while (dr.Read())
                            {
                                objNoPhotoList.objTwoDaySMSList.Add(new NoPhotoSMSEntity
                                {
                                    CustomerName = Convert.ToString(dr["CustomerName"]),
                                    Make = Convert.ToString(dr["MakeName"]),
                                    Model = Convert.ToString(dr["ModelName"]),
                                    InquiryId = Convert.ToString(dr["InquiryId"]),
                                    CustomerNumber = Convert.ToString(dr["CustomerMobile"]),


                                });
                            }
                            Logs.WriteInfoLog("Success in Two days SMS List NoPhotoSMSDAL");
                        }
                        if (dr.NextResult())
                        {
                            objNoPhotoList.objThreeDayMailList = new List<NoPhotoSMSEntity>();
                            while (dr.Read())
                            {
                                objNoPhotoList.objThreeDayMailList.Add(new NoPhotoSMSEntity
                                {
                                    CustomerName = Convert.ToString(dr["CustomerName"]),
                                    Make = Convert.ToString(dr["MakeName"]),
                                    Model = Convert.ToString(dr["ModelName"]),
                                    InquiryId = Convert.ToString(dr["InquiryId"]),
                                    CustomerEmail = Convert.ToString(dr["CustomerEmail"]),

                                });
                            }
                            Logs.WriteInfoLog("Success in Three days Email List NoPhotoSMSDAL");
                        }
                        if (dr.NextResult())
                        {
                            objNoPhotoList.objSevenDayMailList = new List<NoPhotoSMSEntity>();
                            while (dr.Read())
                            {
                                objNoPhotoList.objSevenDayMailList.Add(new NoPhotoSMSEntity
                                {
                                    CustomerName = Convert.ToString(dr["CustomerName"]),
                                    Make = Convert.ToString(dr["MakeName"]),
                                    Model = Convert.ToString(dr["ModelName"]),
                                    InquiryId = Convert.ToString(dr["InquiryId"]),
                                    CustomerEmail = Convert.ToString(dr["CustomerEmail"]),
                                });
                            }
                            Logs.WriteInfoLog("Success in Seven days Email List NoPhotoSMSDAL");
                        }
                        dr.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Logs.WriteInfoLog("Exception in NoPhotoSMSDAL");
                ErrorClass objErr = new ErrorClass(ex, "NoPhotoSMSDAL.SendSMSNoPhoto");
                objErr.SendMail();
            }
            Logs.WriteInfoLog("Ended successfully  NoPhotoSMSDAL");
            return objNoPhotoList;
        }
    }
}
