using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity;
using Carwale.Entity.Enum;
using Carwale.Interfaces;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Carwale.DAL.Customers
{
    public class CustomerRepository<T, TOut> : RepositoryBase, ICustomerRepository<T, TOut>
        where T : Customer
        where TOut : CustomerOnRegister
    {
        public TOut Create(T entity)
        {
            var t = new CustomerOnRegister();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("InsertTempCustomer_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Name", DbType.String, entity.Name));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Email", DbType.String, entity.Email));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Mobile", DbType.String, entity.Mobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int64, entity.CityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_State", DbType.Int32, entity.StateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_passwordsalt", DbType.String, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_passwordhash", DbType.String, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PwdSaltHashStr", DbType.String, entity.PasswordSaltHashStr));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_NewOAuth", DbType.String, string.IsNullOrEmpty(entity.OAuth) ? Convert.DBNull : entity.OAuth));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReceiveNewsletters", DbType.Boolean, entity.ReceiveNewsletters));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_RegistrationDateTime", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_SecurityKey", DbType.String, entity.SecurityKey ?? string.Empty));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_FbId", DbType.Int64, string.IsNullOrEmpty(entity.FacebookId) ? Convert.DBNull : entity.FacebookId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_GplusId", DbType.String, string.IsNullOrEmpty(entity.GoogleId) ? Convert.DBNull : entity.GoogleId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_OpenIDVerified", DbType.Boolean, entity.openUserVerified));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_RegistrationStatus", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsApproved", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_OAuth", DbType.String, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                    t.CustomerId = cmd.Parameters["v_CustomerId"].Value.ToString();
                    t.OAuth = cmd.Parameters["v_OAuth"].Value == DBNull.Value ? "" : cmd.Parameters["v_OAuth"].Value.ToString();
                    t.StatusOnRegister = cmd.Parameters["v_RegistrationStatus"].Value.ToString();
                    bool IsApproved;
                    IsApproved = cmd.Parameters["v_IsApproved"].Value.ToString() == "1";
                    t.IsApproved = IsApproved;
                }
            } // catch SqlException
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, "");
                objErr.LogException();
            } // catch Exception
            return (TOut)t;
        }

        public bool Update(T entity)
        {

            bool returnVal = false;

            try
            {
                var param = new DynamicParameters();
                param.Add("v_CustomerId", Convert.ToInt32(entity.CustomerId));
                param.Add("v_Name", entity.Name.ToString());
                param.Add("v_Email", entity.Email.ToString());
                param.Add("v_Address", entity.Address.ToString());
                param.Add("v_StateId", Convert.ToInt32(entity.StateId));
                param.Add("v_CityId", Convert.ToInt32(entity.CityId));
                param.Add("v_AreaId", 0);
                param.Add("v_Phone1", entity.Phone);
                param.Add("v_Mobile", entity.Mobile.ToString());
                param.Add("v_ReceiveNewsletters", Convert.ToBoolean(entity.ReceiveNewsletters));
                param.Add("v_IsVerified", true);
                param.Add("v_IsFake", false);
                param.Add("v_Status", DbType.Boolean, direction: ParameterDirection.Output);
                using (var con = CarDataMySqlMasterConnection)
                {
                    con.Execute("UpdateCustomerDetails_v16_11_7", param, commandType: CommandType.StoredProcedure);
                    returnVal = Convert.ToBoolean(param.Get<int>("v_Status"));
                }
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, "");
                objErr.LogException();
            }
            return returnVal;
        }

        //New Implementations

        public T GetCustomerFromEmail(string emailId, string oauth = "")
        {
            var customer = new Customer();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetCustomerDetailsByIdOrEmail_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_customerid", DbType.String, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_LoginId", DbType.String, 50, emailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_NewOAuth", DbType.String, 50, !String.IsNullOrEmpty(oauth) ? oauth : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_UserId", DbType.Int64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Email", DbType.String, 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PwdHashSaltStr", DbType.String, 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Name", DbType.String, 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsEmailVerified", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Mobile", DbType.String, 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_OAuth", DbType.String, 50, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.CarDataMySqlMasterConnection);

                    customer.Email = cmd.Parameters["v_Email"].Value.ToString();
                    customer.CustomerId = cmd.Parameters["v_UserId"].Value.ToString();
                    customer.IsEmailVerified = cmd.Parameters["v_IsEmailVerified"].Value == DBNull.Value ? false : Convert.ToBoolean(cmd.Parameters["v_IsEmailVerified"].Value);
                    customer.Name = cmd.Parameters["v_Name"].Value.ToString();
                    customer.PasswordSaltHashStr = cmd.Parameters["v_PwdHashSaltStr"].Value.ToString();
                    customer.Mobile = cmd.Parameters["v_Mobile"].Value.ToString();
                    customer.OAuth = cmd.Parameters["v_OAuth"].Value != DBNull.Value ? cmd.Parameters["v_OAuth"].Value.ToString() : "";
                }
            }
            catch (MySqlException err)
            {
                var objErr = new ExceptionHandler(err, "SP GetCustomerDetailsByIdOrEmail_v16_11_7");
                objErr.LogException();
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, "");
                objErr.LogException();
            }
            return (T)customer;
        }

        public T GetCustomerFromAuthKey(string oauth)
        {
            var customer = new Customer();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("cwmasterdb.GetCustomerDetailsByAuth_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_OAuth", DbType.String, 40, oauth));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.CarDataMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            customer = new Customer()
                            {
                                CustomerId = dr["Id"].ToString(),
                                Email = dr["email"].ToString(),
                                Name = dr["Name"].ToString(),
                                IsEmailVerified = Convert.ToBoolean(dr["IsEmailVerified"].ToString()),
                                Mobile = dr["Mobile"].ToString()
                            };
                        }
                    }
                }
            }
            catch (MySqlException err)
            {
                var objErr = new ExceptionHandler(err, "SP GetCustomerFromAuthKey");
                objErr.LogException();
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, "SP GetCustomerFromAuthKey");
                objErr.LogException();
            }
            return (T)customer;
        }

        public T GetCustomerFromCustomerId(string customerId)
        {
            var customer = new Customer();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetCustomerDetailsByIdOrEmail_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_LoginId", DbType.String, 50, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_NewOAuth", DbType.String, 50, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_UserId", DbType.Int64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Email", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PwdHashSaltStr", DbType.String, 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Name", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsEmailVerified", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Mobile", DbType.String, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_OAuth", DbType.String, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.CarDataMySqlReadConnection);

                    customer.Email = cmd.Parameters["v_Email"].Value.ToString();
                    customer.CustomerId = cmd.Parameters["v_UserId"].Value.ToString();
                    customer.Name = cmd.Parameters["v_Name"].Value.ToString();
                    customer.PasswordSaltHashStr = cmd.Parameters["v_PwdHashSaltStr"].Value.ToString();
                    customer.Mobile = cmd.Parameters["v_Mobile"].Value.ToString();
                    customer.OAuth = cmd.Parameters["v_OAuth"].Value != DBNull.Value ? cmd.Parameters["v_OAuth"].Value.ToString() : "";
                }
            }
            catch (MySqlException err)
            {
                var objErr = new ExceptionHandler(err, "SP GetCustomerDetailsByIdOrEmail_v16_11_7");
                objErr.LogException();
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, "SP GetCustomerDetailsByIdOrEmail");
                objErr.LogException();
            }
            return (T)customer;
        }

        public T GetCustomerFromSecurityKey(string key)
        {
            Customer customer = null;
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("v_securitykey", key);

                using (var con = CarDataMySqlReadConnection)
                {
                    customer = con.Query<Customer>("GetCustomerBySecurityKey", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return (T)customer;
        }

        public bool ResetPassword(string CustomerId, string PasswordHashSalt, string newOauth)
        {
            bool retval = false;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_CustomerId", Convert.ToInt64(CustomerId));
                param.Add("v_PasswordHashSalt", PasswordHashSalt);
                param.Add("v_NewOauth", newOauth);
                param.Add("v_Status", DbType.Boolean, direction: ParameterDirection.Output);
                using (var con = CarDataMySqlMasterConnection)
                {
                    con.Execute("ResetPassword_v16_11_7", param, commandType: CommandType.StoredProcedure);
                    retval = Convert.ToBoolean(param.Get<int>("v_Status"));
                }
            }
            catch (MySqlException err)
            {
                var objErr = new ExceptionHandler(err, "ResetPassword(string CustomerId, string PasswordHashSalt, string Password)");
                objErr.LogException();
            } // catch SqlException
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, "ResetPassword(string CustomerId, string PasswordHashSalt, string Password)");
                objErr.LogException();
            } // catch Exception
            return retval;
        }

        public string SavePasswordChangeAT(string Email, string AccessToken)
        {
            string customerId = "-1";
            var param = new DynamicParameters();
            param.Add("v_Email", Email);
            param.Add("v_AccessToken", AccessToken);
            param.Add("v_CustomerId", DbType.Int64, direction: ParameterDirection.Output);
            using (var con = CarDataMySqlMasterConnection)
            {
                con.Execute("SavePasswordChangeAT_v16_11_7", param, commandType: CommandType.StoredProcedure);
                customerId = param.Get<int>("v_CustomerId").ToString();
            }
            return customerId;
        }

        public string GetCustomerIdByAccessToken(string AccessToken, out int MinutesDiff)
        {
            MinutesDiff = -1;
            string customerId = "-1";
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetCustomerIdByAccessToken_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_AccessToken", DbType.String, 8000, AccessToken));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MinutesDiff", DbType.String, 8000, ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.CarDataMySqlReadConnection);
                    customerId = cmd.Parameters["v_CustomerId"].Value.ToString();
                    if (!int.TryParse(cmd.Parameters["v_MinutesDiff"].Value.ToString(), out MinutesDiff))
                        MinutesDiff = -1;
                }
            }
            catch (MySqlException err)
            {
                var objErr = new ExceptionHandler(err, "SP SavePasswordChangeAT");
                objErr.LogException();
            } // catch SqlException
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, "SP SavePasswordChangeAT");
                objErr.LogException();
            } // catch Exception
            return customerId;
        }

        public bool CreateRememberMeSession(CustomerRememberMe custrm)
        {
            bool retval = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("createremembermesession_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_customerid", DbType.Int64, custrm.CustomerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_identifier", DbType.String, custrm.Identifier));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_accesstoken", DbType.String, custrm.AccessToken));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ipaddress", DbType.String, custrm.IPAddress));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_useragent", DbType.String, custrm.UserAgent));
                    if (MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.CarDataMySqlMasterConnection) > 0)
                        retval = true;
                }

            }

            catch (MySqlException err)
            {
                var objErr = new ExceptionHandler(err, "SP CreateRememberMeSession");
                objErr.LogException();
            }
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, "SP CreateRememberMeSession");
                objErr.LogException();
            }
            return retval;
        }

        public string UseActiveRememberMeSession(CustomerRememberMe custrm, string NewAccessToken)
        {
            string retval = "N";
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("cwmasterdb.UseActiveRememberMesession_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_customerid", DbType.Int64, custrm.CustomerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_identifier", DbType.String, custrm.Identifier));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_accesstoken", DbType.String, custrm.AccessToken));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_newaccesstoken", DbType.String, NewAccessToken));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ipaddress", DbType.String, custrm.IPAddress));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_useragent", DbType.String, custrm.UserAgent));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_returnval", DbType.String, ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                    retval = cmd.Parameters["v_returnval"].Value.ToString();
                }
            }
            catch (SqlException err)
            {
                var objErr = new ExceptionHandler(err, "SP UseActiveRememberMeSession");
                objErr.LogException();
            } // catch SqlException
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, "SP UseActiveRememberMeSession");
                objErr.LogException();
            } // catch Exception

            return retval;
        }

        public bool EndRememberMeSession(string customerId, string identifier)
        {
            bool retval = false;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_CustomerId", customerId);
                param.Add("v_Identifier", identifier);
                using (var con = CarDataMySqlMasterConnection)
                {
                    var retvalnew = con.Execute("EndRememberMeSession_v16_11_7", param, commandType: CommandType.StoredProcedure);
                    retval = Convert.ToBoolean(retvalnew);
                }
            }
            catch (MySqlException err)
            {
                var objErr = new ExceptionHandler(err, "SP EndRememberMeSession");
                objErr.LogException();
            } // catch SqlException
            catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, "SP EndRememberMeSession");
                objErr.LogException();
            } // catch Exception
            return retval;
        }

        /// <summary>
        /// Written by Satish Sharma on 11-Sep-2015
        /// This function makes  entry on table "DoNotSendEmail"
        /// </summary>
        /// <param name="email">email that user want to unsubscribe from newsletter</param>
        /// <returns>bool</returns>
        public bool UnsubscribeNewsletter(string email)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("InsertDoNotSendEmail_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Email", DbType.String, email.ToLower()));
                    return MySqlDatabase.UpdateQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                }
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "Carwale.DAL.Customers.CustomerRepository.UnsubscribeNewsletter");
                objErr.LogException();
            }
            return false;
        }

        public void UpdateSourceId(EnumTableType source, string id)
        {
            string sourceId = ConfigurationManager.AppSettings["SourceId"].ToString();
            try
            {
                if (sourceId != "1" && id != "")	//if source is not carwale then only proceed
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("UpdateSourceId_v16_11_7"))
                    {
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, id != null ? Convert.ToInt64(id) : 0));
                        switch (source)
                        {
                            case EnumTableType.Customers:
                                cmd.Parameters.Add(DbFactory.GetDbParam("v_Source", DbType.Int16, 1));
                                break;
                            case EnumTableType.CustomerSellInquiries:
                                cmd.Parameters.Add(DbFactory.GetDbParam("v_Source", DbType.Int16, 2));
                                break;
                            case EnumTableType.PriceQuote:
                                cmd.Parameters.Add(DbFactory.GetDbParam("v_Source", DbType.Int16, 3));
                                break;
                            case EnumTableType.CustomerReviews:
                                cmd.Parameters.Add(DbFactory.GetDbParam("v_Source", DbType.Int16, 4));
                                break;
                            case EnumTableType.CustomerReviewsComments:
                                cmd.Parameters.Add(DbFactory.GetDbParam("v_Source", DbType.Int16, 5));
                                break;
                            case EnumTableType.AppCustomers:
                                cmd.Parameters.Add(DbFactory.GetDbParam("v_Source", DbType.Int16, EnumTableType.AppCustomers));
                                break;
                        }
                        MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                    }

                }
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "Carwale.DAL.Customers.CustomerRepository.UpdateSourceId(EnumTableType source , string id)");
                objErr.LogException();
            }
        }

        public bool UpdateEmailVerfication(bool isEmailVerified, int userId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("UpdateEmailVerification"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_userId", DbType.Int32, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_isEmailVerified", DbType.Boolean, isEmailVerified));
                    return MySqlDatabase.UpdateQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }
    }
}
