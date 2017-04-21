using Bikewale.Utility;
using Consumer;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

namespace Bikewale.UserReviewsRatingEmail
{
    /// <summary>
    /// Created by : Aditi Srivastava on 15 Apr 2017
    /// Summary    : Functions to get list of users to send reminders to after 7 days
    /// </summary>
    public class UserReviewsRatingEmailDAL
    {
        /// <summary>
        /// Created by : Aditi Srivastava on 18 Apr 2017
        /// Summary    : Get list of users to send email after rating submission
        /// </summary>
        public IEnumerable<UserReviewsRatingEmailEntity> GetUserList()
        {
            Logs.WriteInfoLog("Started UserReviewsRatingEmailDAL");
            ICollection<UserReviewsRatingEmailEntity> userList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getcustomerdetailsforratingmail";

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            userList = new Collection<UserReviewsRatingEmailEntity>();
                            while (dr.Read())
                            {
                                userList.Add(new UserReviewsRatingEmailEntity
                                {
                                    ReviewId = SqlReaderConvertor.ToUInt32(dr["ReviewId"]),
                                    MakeName = Convert.ToString(dr["MakeName"]),
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    CustomerId = SqlReaderConvertor.ToUInt32(dr["CustomerId"]),
                                    CustomerName = Convert.ToString(dr["CustomerName"]),
                                    CustomerEmail = Convert.ToString(dr["CustomerEmail"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch
            {
                Logs.WriteInfoLog("Exception in UserReviewsRatingEmailDAL");
            }
            Logs.WriteInfoLog("Successfully executed UserReviewsRatingEmailDAL");
            return userList;
        }

    }

}


