
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities.PopularComparisions;
using BikewaleOpr.Interface.PopularComparisions;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace BikewaleOpr.DALs.PopularComparisions
{
    /// <summary>
    /// 
    /// </summary>
    public class PopularBikeComparisionsRepository : IPopularBikeComparisions
    {
        /// <summary>
        /// Created By : Sushil Kumar on 26th Oct 2016 
        /// Description : Save comparision list to db 
        /// </summary>
        /// <param name="compareId"></param>
        /// <param name="versionId1"></param>
        /// <param name="versionId2"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public bool SaveBikeComparision(ushort compareId, uint versionId1, uint versionId2, bool isActive)
        {
            bool isDataSaved = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "bikecomparisionlist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int16, compareId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid1", DbType.UInt32, versionId1));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid2", DbType.UInt32, versionId2));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_entrydate", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, isActive));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_compid", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_status", DbType.Int16, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    isDataSaved = true;

                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, string.Format("BikewaleOpr.DALs.PopularComparisions.SaveBikeComparision_{0}_Version1_{1}_Version2_{2}", compareId, versionId1, versionId2));

            } // catch Exception

            return isDataSaved;
        }

        // Shows all the data from the Con_BikeComparisonList table
        /// <summary>
        /// Modified By : Sadhana Upadhyay on 10th Feb 2014
        /// Summary : to get Image HostUrl, ImagePath, Image Name
        /// Modified By : Sadhana Upadhyay on 13th Feb 2014
        /// Summary : Replaced Inline Query with procedure
        /// </summary>
        public IEnumerable<PopularBikeComparision> GetBikeComparisions()
        {
            IList<PopularBikeComparision> objBikeCamparisions = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "con_getbikecomparisonlisting";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            objBikeCamparisions = new List<PopularBikeComparision>();
                            while (dr.Read())
                            {
                                PopularBikeComparision _objBikeComparision = new PopularBikeComparision();
                                _objBikeComparision.ComparisionId = SqlReaderConvertor.ParseToUInt32(dr["id"]);
                                _objBikeComparision.Bike1 = Convert.ToString(dr["bike1"]);
                                _objBikeComparision.Bike2 = Convert.ToString(dr["bike2"]);
                                _objBikeComparision.ModelId1 = SqlReaderConvertor.ToUInt32(dr["modelid1"]);
                                _objBikeComparision.ModelId2 = SqlReaderConvertor.ToUInt32(dr["modelid2"]);
                                _objBikeComparision.MakeId1 = SqlReaderConvertor.ToUInt32(dr["makeid1"]);
                                _objBikeComparision.MakeId2 = SqlReaderConvertor.ToUInt32(dr["makeid2"]);
                                _objBikeComparision.VersionId1 = SqlReaderConvertor.ToUInt32(dr["versionid1"]);
                                _objBikeComparision.VersionId2 = SqlReaderConvertor.ToUInt32(dr["versionid2"]);
                                _objBikeComparision.HostUrl1 = Convert.ToString(dr["hosturl1"]);
                                _objBikeComparision.HostUrl2 = Convert.ToString(dr["hosturl2"]);
                                _objBikeComparision.OriginalImagePath1 = Convert.ToString(dr["originalimagepath1"]);
                                _objBikeComparision.OriginalImagePath2 = Convert.ToString(dr["originalimagepath2"]);
                                _objBikeComparision.EntryDate = SqlReaderConvertor.ToDateTime(dr["entrydate"]);
                                _objBikeComparision.IsActive = SqlReaderConvertor.ToBoolean(dr["isactive"]);
                                _objBikeComparision.PriorityOrder = SqlReaderConvertor.ToUInt16(dr["displaypriority"]);
                                objBikeCamparisions.Add(_objBikeComparision);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DALs.PopularComparisions.GetBikeComparisions");
            }
            return objBikeCamparisions;
        }


        /// <summary>
        /// Created By : Sadhana Upadhyay on 19th Feb 2014
        /// Summary : To delete Compare Bike records
        /// </summary>
        /// <param name="deleteId"></param>
        public bool DeleteCompareBike(uint deleteId)
        {
            bool isDeleted = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("deletecomparebikedate"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.UInt32, deleteId));
                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                    isDeleted = true;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "BikewaleOpr.DALs.PopularComparisions.DeleteCompareBike_" + deleteId);
                objErr.SendMail();
            }

            return isDeleted;
        }   //End of DeleteCompareBike

        /// <summary>
        /// Created By : Sadhana Upadhyay on 19th Feb 2014
        /// Summary : To update Compare Bike priority
        /// </summary>
        /// <param name="deleteId"></param>
        public bool UpdatePriorities(string prioritiesList)
        {
            bool isUpdated = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "setbikecomparisonpriority";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_prioritieslist", DbType.String, 1000, prioritiesList));

                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);

                    isUpdated = true;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "BikewaleOpr.DALs.PopularComparisions.UpdatePriorities_" + prioritiesList);
                objErr.SendMail();
            }

            return isUpdated;

        }   //End of UpdatePriorities

    }
}
