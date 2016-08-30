using Bikewale.Entities.Used;
using Bikewale.Interfaces.Used;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web;

namespace Bikewale.DAL.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 29th August 2016
    /// Description : Used Bikes details repository for used bikes section
    /// </summary>
    public class IUsedBikeDetailsRepository : IUsedBikeDetails
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <returns></returns>
        public ClassifiedInquiryDetails GetProfileDetails(uint inquiryId)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<BikeDetailsMin> GetSimilarBikes(uint inquiryId, uint cityId, uint modelId, ushort topCount)
        {
            List<BikeDetailsMin> similarBikeDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_similarbikes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryId", DbType.UInt32, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.UInt32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.UInt32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.UInt16, topCount));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            similarBikeDetails = new List<BikeDetailsMin>();
                            while (dr.Read())
                            {
                                similarBikeDetails.Add(new BikeDetailsMin()
                                {
                                    ProfileId = Convert.ToString(dr["ProfileId"]),
                                    ModelYear = Convert.ToString(dr["makeyear"]),
                                    OwnerType = Convert.ToString(dr["Owner"]),
                                    KmsDriven = Convert.ToUInt32(dr["Kilometers"]),
                                    AskingPrice = Convert.ToUInt32(dr["Price"]),
                                    RegisteredAt = Convert.ToString(dr["cityname"]),
                                    Photo = new BikePhoto()
                                    {
                                        HostUrl = Convert.ToString(dr["HostUrl"]),
                                        OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                        IsMain = true
                                    }
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return similarBikeDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<OtherUsedBikeDetails> GetOtherBikesByCityId(uint inquiryId, uint cityId, ushort topCount)
        {
            List<OtherUsedBikeDetails> similarBikeDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("classified_otherbikesbycity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_inquiryId", DbType.UInt32, inquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.UInt32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.UInt16, topCount));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            similarBikeDetails = new List<OtherUsedBikeDetails>();
                            while (dr.Read())
                            {
                                similarBikeDetails.Add(new OtherUsedBikeDetails()
                                {
                                    ProfileId = Convert.ToString(dr["ProfileId"]),
                                    ModelYear = Convert.ToString(dr["makeyear"]),
                                    OwnerType = Convert.ToString(dr["Owner"]),
                                    KmsDriven = Convert.ToUInt32(dr["Kilometers"]),
                                    AskingPrice = Convert.ToUInt32(dr["Price"]),
                                    RegisteredAt = Convert.ToString(dr["cityname"]),
                                    MakeName = Convert.ToString(dr["MakeName"]),
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                    ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                    CityMaskingName = Convert.ToString(dr["CityMaskingName"]),
                                    BikeName = string.Format("{0} {1}", Convert.ToString(dr["MakeName"]), Convert.ToString(dr["ModelName"])),
                                    Photo = new BikePhoto()
                                    {
                                        HostUrl = Convert.ToString(dr["HostUrl"]),
                                        OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                        IsMain = true
                                    }
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return similarBikeDetails;
        }
    }
}
