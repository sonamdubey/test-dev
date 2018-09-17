using Bikewale.Entities.Customer;
using Bikewale.Interfaces.Customer;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;

namespace Bikewale.DAL.Customer
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class CustomerRepository<T, U> : ICustomerRepository<T, U> where T : CustomerEntity, new()
    {
        /// <summary>
        /// Function to save the new customer into bikewale database
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        #region Register new customer
        public U Add(T t)
        {
            U customerId = default(U);
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "registercustomer_new";
                    // LogLiveSps.LogSpInGrayLog(cmd);


                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, 50, t.CustomerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, 50, t.CustomerEmail.Trim().ToLower()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, 20, t.CustomerMobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customercityid", DbType.Int32, String.IsNullOrEmpty(t.cityDetails.CityId.ToString()) ? Convert.DBNull : t.cityDetails.CityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_passwordsalt", DbType.String, 10, t.PasswordSalt));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_passwordhash", DbType.String, 64, t.PasswordHash));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, CurrentUser.GetClientIP()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    customerId = (U)Convert.ChangeType(cmd.Parameters["par_customerid"].Value, typeof(U));
                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return customerId;
        }
        #endregion

        #region Update All details of customer
        public bool Update(T t)
        {
            bool returnVal = false;
            try
            {


                using (DbCommand cmd = DbFactory.GetDBCommand("updatecustomerdetails_07062017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, t.CustomerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbType.String, 100, t.CustomerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 100, t.CustomerEmail));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 50, t.CustomerMobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, t.cityDetails != null ? t.cityDetails.CityId : 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_areaid", DbType.Byte, 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_receivenewsletters", DbType.Int32, 1));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isverified", DbType.Boolean, 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_phone1", DbType.String, 50, Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_address", DbType.String, 100, Convert.DBNull));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    returnVal = true;
                }
            }
            catch (SqlException err)
            {
                ErrorClass.LogError(err, String.Format("CustomerRepository.Update({0})", Newtonsoft.Json.JsonConvert.SerializeObject(t)));
                returnVal = false;
            }
            return returnVal;
        }
        #endregion

        #region Delete Customer
        public bool Delete(U id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Get All Customers information
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll()
        {
            List<T> tList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "fetchallcustomerdetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            tList = new List<T>();

                            while (dr.Read())
                            {
                                if (!String.IsNullOrEmpty(dr["CustomerEmail"].ToString()))
                                {
                                    T t = new T();

                                    t.CustomerId = Convert.ToUInt64(dr["CustomerId"]);
                                    t.CustomerName = Convert.ToString(dr["CustomerName"]);
                                    t.CustomerEmail = Convert.ToString(dr["CustomerEmail"]);
                                    t.CustomerMobile = Convert.ToString(dr["CustomerMobile"]);
                                    t.Password = Convert.ToString(dr["Password"]);
                                    t.PasswordSalt = Convert.ToString(dr["PasswordSalt"]);
                                    t.PasswordHash = Convert.ToString(dr["PasswordHash"]);
                                    t.cityDetails.CityId = Convert.ToUInt32(dr["CityId"]);
                                    t.cityDetails.CityName = Convert.ToString(dr["CityName"]);
                                    t.stateDetails.StateId = Convert.ToUInt32(dr["StateId"]);
                                    t.stateDetails.StateName = Convert.ToString(dr["StateName"]);
                                    t.IsVerified = Convert.ToBoolean(dr["IsVerified"]);

                                    tList.Add(t);
                                }
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return tList;
        }
        #endregion

        #region Get Customer information by id
        public T GetById(U id)
        {
            T t = default(T);
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "fetchcustomerdetailsbyid";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, 20, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_statename", DbType.String, 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityname", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_password", DbType.String, 20, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_passwordsalt", DbType.String, 10, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_passwordhash", DbType.String, 64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isverified", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isexist", DbType.Boolean, ParameterDirection.Output));
                    // LogLiveSps.LogSpInGrayLog(cmd);

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);
                    t = new T();

                    if (Convert.ToBoolean(cmd.Parameters["par_isexist"].Value))
                    {
                        t.CustomerId = Convert.ToUInt64(id);
                        t.CustomerName = Convert.ToString(cmd.Parameters["par_customername"].Value);
                        t.CustomerEmail = Convert.ToString(cmd.Parameters["par_customeremail"].Value);
                        t.CustomerMobile = Convert.ToString(cmd.Parameters["par_customermobile"].Value);
                        t.cityDetails.CityId = Convert.ToUInt32(cmd.Parameters["par_cityid"].Value);
                        t.cityDetails.CityName = Convert.ToString(cmd.Parameters["par_cityname"].Value);
                        t.stateDetails.StateId = Convert.ToUInt32(cmd.Parameters["par_stateid"].Value);
                        t.stateDetails.StateName = Convert.ToString(cmd.Parameters["par_statename"].Value);
                        t.Password = Convert.ToString(cmd.Parameters["par_password"].Value);
                        t.PasswordSalt = Convert.ToString(cmd.Parameters["par_passwordsalt"].Value);
                        t.PasswordHash = Convert.ToString(cmd.Parameters["par_passwordhash"].Value);
                        t.IsVerified = Convert.ToBoolean(cmd.Parameters["par_IsVerified"].Value);
                    }

                    t.IsExist = Convert.ToBoolean(cmd.Parameters["par_isexist"].Value);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return t;
        }
        #endregion

        #region Get Customer information by email
        public T GetByEmail(string emailId)
        {
            T t = default(T);
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "fetchcustomerdetailsbyemail";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, 100, emailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, 20, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_statename", DbType.String, 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityname", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_password", DbType.String, 20, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_passwordsalt", DbType.String, 10, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_passwordhash", DbType.String, 64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isverified", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isexist", DbType.Boolean, ParameterDirection.Output));

                    // LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);

                    t = new T();

                    if (Convert.ToBoolean(cmd.Parameters["par_isexist"].Value))
                    {
                        t.CustomerEmail = emailId;
                        t.CustomerId = Convert.ToUInt64(cmd.Parameters["par_customerid"].Value);
                        t.CustomerName = Convert.ToString(cmd.Parameters["par_customername"].Value);
                        t.CustomerMobile = Convert.ToString(cmd.Parameters["par_customermobile"].Value);
                        t.cityDetails.CityId = Convert.ToUInt32(cmd.Parameters["par_cityid"].Value);
                        t.cityDetails.CityName = Convert.ToString(cmd.Parameters["par_cityname"].Value);
                        t.stateDetails.StateId = Convert.ToUInt32(cmd.Parameters["par_stateid"].Value);
                        t.stateDetails.StateName = Convert.ToString(cmd.Parameters["par_statename"].Value);
                        t.Password = Convert.ToString(cmd.Parameters["par_password"].Value);
                        t.PasswordSalt = Convert.ToString(cmd.Parameters["par_passwordsalt"].Value);
                        t.PasswordHash = Convert.ToString(cmd.Parameters["par_passwordhash"].Value);
                        t.IsVerified = Convert.ToBoolean(cmd.Parameters["par_isverified"].Value);
                    }
                    t.IsExist = Convert.ToBoolean(cmd.Parameters["par_isexist"].Value);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.Customer.CustomerRepository_emailId_{0}", emailId));
            }

            return t;
        }
        #endregion

        #region Update customer mobile number and name
        public void UpdateCustomerMobileNumber(string mobile, string email, string name = null)
        {

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("updatecustomermobile"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 20, mobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 100, email));

                    if (!String.IsNullOrEmpty(name)) { cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbType.String, 50, name)); }

                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn(sqlEx.Message);
                ErrorClass.LogError(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }
        #endregion

        #region Update customer password salt and hash
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="passwordSalt"></param>
        /// <param name="passwordHash"></param>
        public void UpdatePasswordSaltHash(U customerId, string passwordSalt, string passwordHash)
        {

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "updatecustomerpassword";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_salt", DbType.String, 10, passwordSalt));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hash", DbType.String, 64, passwordHash));

                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }
        #endregion

        #region Save password recovery token
        public void SavePasswordRecoveryToken(U customerId, string token)
        {

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "savepasswordrecoverytoken";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_token", DbType.String, 200, token));

                    MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }
        #endregion

        #region Get password recovery token is valid or not.
        public bool IsValidPasswordRecoveryToken(U customerId, string token)
        {
            bool isValidtoken = false;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "checkvalidpasswordtoken";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_token", DbType.String, 200, token));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isvalidtoken", DbType.Boolean, ParameterDirection.Output));

                    // LogLiveSps.LogSpInGrayLog(cmd);


                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);
                    isValidtoken = Convert.ToBoolean(cmd.Parameters["par_isvalidtoken"].Value);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return isValidtoken;
        }
        #endregion

        #region Deactivate password recovery token
        public void DeactivatePasswordRecoveryToken(U customerId)
        {

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "updatepasswordrecoverytokenstatus";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, customerId));

                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }
        #endregion


        /// <summary>
        /// Created By : Sangram Nandkhile Upadhyay on 13 Oct 2014
        /// Summary : To get isfake flag by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool IsFakeCustomer(ulong customerId)
        {
            bool isFake = false;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "checkfakecustomerbyid";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.UInt64, customerId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            isFake = Convert.ToBoolean(dr["IsFake"]);
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("bikeWale.DAL.Used.SellBikesRepository.IsFakeCustomer({0})", customerId));
            }

            return isFake;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 07 Jun 2017
        /// Description :   Returns customer details by email or mobile
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public T GetByEmailMobile(string emailId, string mobile)
        {
            T t = default(T);
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "fetchcustomerdetailsbyemailmobile";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, 100, emailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, 20, mobile));
                    t = new T();

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            t.IsExist = true;
                            t.CustomerEmail = Convert.ToString(dr["email"]);
                            t.CustomerId = SqlReaderConvertor.ToUInt64(dr["customerid"]);
                            t.CustomerName = Convert.ToString(dr["name"]);
                            t.CustomerMobile = Convert.ToString(dr["mobile"]);
                            t.cityDetails.CityId = SqlReaderConvertor.ToUInt32(dr["cityid"]);
                            t.cityDetails.CityName = Convert.ToString(dr["cityname"]);
                            t.stateDetails.StateId = SqlReaderConvertor.ToUInt32(dr["stateid"]);
                            t.stateDetails.StateName = Convert.ToString(dr["statename"]);
                            t.Password = Convert.ToString(dr["password"]);
                            t.PasswordSalt = Convert.ToString(dr["passwordsalt"]);
                            t.PasswordHash = Convert.ToString(dr["passwordhash"]);
                            t.IsVerified = SqlReaderConvertor.ToBoolean(dr["isverified"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetByEmailMobile({0},{1})", emailId, mobile));
            }

            return t;
        }
    }   // class
}   // namespace
