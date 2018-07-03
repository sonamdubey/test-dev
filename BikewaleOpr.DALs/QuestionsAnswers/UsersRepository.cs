using Bikewale.Notifications;
using BikewaleOpr.Entity.Users;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace BikewaleOpr.DALs.QuestionsAnswers
{
    public class UsersRepository : Interface.QuestionsAnswers.IUsersRepository
    {
        /// <summary>
        /// Created by  : Sanskar Gupta on 19 June 2018
        /// Description : Function to return a list of all the internal QnA users.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetInternalUsers()
        {
            ICollection<User> internalUsers = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getinternalusers"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            internalUsers = new List<User>();
                            while (dr.Read())
                            {
                                User user = new User();
                                user.Id = Convert.ToUInt32(dr["id"]);
                                user.Name = Convert.ToString(dr["name"]);
                                user.Email = Convert.ToString(dr["email"]);

                                internalUsers.Add(user);
                            }

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikewaleOpr.DALs.QuestionsAnswers.UsersRepository.GetInternalUsers"));
            }
            return internalUsers;
        }
    }
}
