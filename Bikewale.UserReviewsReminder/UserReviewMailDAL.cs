﻿using Bikewale.Utility;
using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;


namespace Bikewale.UserReviewsCommunication
{
    /// <summary>
    /// Created by : Aditi Srivastava on 15 Apr 2017
    /// Summary    : Functions to get list of users to send reminders to after 7 days
    /// </summary>
    public class UserReviewMailDAL
    {
        /// <summary>
        /// Created by : Aditi Srivastava on 15 Apr 2017
        /// Summary    : Get list of users to send reminders to after 7 days
        /// </summary>
        public IEnumerable<UserReviewMailEntity> GetUserList()
        {
            Logs.WriteInfoLog("Started UserReviewMailDAL");
            ICollection<UserReviewMailEntity> userList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getcustomerdetailsforreviewreminder";

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            userList = new Collection<UserReviewMailEntity>();
                            while (dr.Read())
                            {
                                userList.Add(new UserReviewMailEntity
                                {
                                    ReviewId = SqlReaderConvertor.ToUInt32(dr["ReviewId"]),
                                    MakeName = Convert.ToString(dr["MakeName"]),
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    CustomerId = SqlReaderConvertor.ToUInt32(dr["CustomerId"]),
                                    CustomerName = Convert.ToString(dr["CustomerName"]),
                                    CustomerEmail = Convert.ToString(dr["CustomerEmail"]),
                                    EntryDate = SqlReaderConvertor.ToDateTime(dr["EntryDate"]),
                                    MakeId=SqlReaderConvertor.ToUInt32(dr["MakeId"]),
                                    ModelId=SqlReaderConvertor.ToUInt32(dr["ModelId"]),
                                    OverAllRating=Convert.ToString(dr["OverAllRating"]),
                                    PriceRangeId=SqlReaderConvertor.ToUInt32(dr["PriceRangeId"]),
                                    PageSourceId=SqlReaderConvertor.ToUInt32(dr["SourceId"]),
                                    IsFake=SqlReaderConvertor.ToBoolean(dr["IsFake"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch
            {
                Logs.WriteInfoLog("Exception in UserReviewMailDAL");
            }
            Logs.WriteInfoLog("Successfully executed UserReviewMailDAL");
            return userList;
        }

       
    }

}
