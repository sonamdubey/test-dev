using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Bikewale.Interfaces.Customer;
using Bikewale.Entities.Customer;
using Bikewale.Notifications;
using Bikewale.CoreDAL;
using System.Data.Common;

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

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50 , t.CustomerName));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50 , t.CustomerEmail.Trim().ToLower()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 20 , t.CustomerMobile));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customercityid", DbParamTypeMapper.GetInstance[SqlDbType.Int] , String.IsNullOrEmpty(t.cityDetails.CityId.ToString()) ? Convert.DBNull : t.cityDetails.CityId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_passwordsalt", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 10 , t.PasswordSalt));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_passwordhash", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 64 , t.PasswordHash));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 40 , string.IsNullOrEmpty(t.ClientIP) ? Convert.DBNull : t.ClientIP));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt] , ParameterDirection.Output));

                        MySqlDatabase.ExecuteNonQuery(cmd);
                        customerId = (U)Convert.ChangeType(cmd.Parameters["par_customerid"].Value, typeof(U)); 
                    }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return customerId;
        }
        #endregion

        #region Update All details of customer
        public bool Update(T t)
        {
            throw new NotImplementedException();
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
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "FetchAllCustomerDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
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
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return tList;
        }
        #endregion

        #region Get Customer information by id
        public T GetById(U id)
        {
            T t = default(T);
            Database db = null;
            try
            {
                db = new Database();

                using (SqlConnection conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "FetchCustomerDetailsById";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = id;
                        cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@CustomerEmail", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@CustomerMobile", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@StateId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@StateName", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@CityId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@CityName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Password", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@PasswordSalt", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@PasswordHash", SqlDbType.VarChar, 64).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@IsVerified", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@IsExist", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        t = new T();

                        if (Convert.ToBoolean(cmd.Parameters["@IsExist"].Value))
                        {
                            t.CustomerId = Convert.ToUInt64(id);
                            t.CustomerName = Convert.ToString(cmd.Parameters["@CustomerName"].Value);
                            t.CustomerEmail = Convert.ToString(cmd.Parameters["@CustomerEmail"].Value);
                            t.CustomerMobile = Convert.ToString(cmd.Parameters["@CustomerMobile"].Value);
                            t.cityDetails.CityId = Convert.ToUInt32(cmd.Parameters["@CityId"].Value);
                            t.cityDetails.CityName = Convert.ToString(cmd.Parameters["@CityName"].Value);
                            t.stateDetails.StateId = Convert.ToUInt32(cmd.Parameters["@StateId"].Value);
                            t.stateDetails.StateName = Convert.ToString(cmd.Parameters["@StateName"].Value);
                            t.Password = Convert.ToString(cmd.Parameters["@Password"].Value);
                            t.PasswordSalt = Convert.ToString(cmd.Parameters["@PasswordSalt"].Value);
                            t.PasswordHash = Convert.ToString(cmd.Parameters["@PasswordHash"].Value);
                            t.IsVerified = Convert.ToBoolean(cmd.Parameters["@IsVerified"].Value);
                        }

                        t.IsExist = Convert.ToBoolean(cmd.Parameters["@IsExist"].Value);
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, emailId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt] , ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50 , ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 20 , ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbParamTypeMapper.GetInstance[SqlDbType.Int] , ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_statename", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30 , ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbParamTypeMapper.GetInstance[SqlDbType.Int] , ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50 , ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_password", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 20 , ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_passwordsalt", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 10 , ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_passwordhash", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 64 , ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_isverified", DbParamTypeMapper.GetInstance[SqlDbType.Bit] , ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_isexist", DbParamTypeMapper.GetInstance[SqlDbType.Bit] , ParameterDirection.Output));

                       MySqlDatabase.ExecuteNonQuery(cmd);

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
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return t;
        }
        #endregion

        #region Update customer mobile number and name
        public void UpdateCustomerMobileNumber(string mobile, string email, string name = null)
        {
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand("UpdateCustomerMobile"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 20).Value = mobile;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email;

                    if (!String.IsNullOrEmpty(name)) { cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = name; }

                    db.UpdateQry(cmd);
                }
            }
            catch (SqlException sqlEx)
            {
                HttpContext.Current.Trace.Warn(sqlEx.Message);
                ErrorClass errObj = new ErrorClass(sqlEx, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "UpdateCustomerPassword";

                    cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = customerId;
                    cmd.Parameters.Add("@Salt", SqlDbType.VarChar, 10).Value = passwordSalt;
                    cmd.Parameters.Add("@Hash", SqlDbType.VarChar, 64).Value = passwordHash;

                    db.UpdateQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
        }
        #endregion

        #region Save password recovery token
        public void SavePasswordRecoveryToken(U customerId, string token)
        {
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SavePasswordRecoveryToken";

                    cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = customerId;
                    cmd.Parameters.Add("@Token", SqlDbType.VarChar, 200).Value = token;

                    db = new Database();

                    db.InsertQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
        }
        #endregion

        #region Get password recovery token is valid or not.
        public bool IsValidPasswordRecoveryToken(U customerId, string token)
        {
            bool isValidtoken = false;
            Database db = null;
            try
            {
                db = new Database();

                using (SqlConnection conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "CheckValidPasswordToken";
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = customerId;
                        cmd.Parameters.Add("@Token", SqlDbType.VarChar, 200).Value = token;
                        cmd.Parameters.Add("@IsValidToken", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        conn.Open();

                        cmd.ExecuteNonQuery();

                        isValidtoken = Convert.ToBoolean(cmd.Parameters["@IsValidtoken"].Value);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return isValidtoken;
        }
        #endregion

        #region Deactivate password recovery token
        public void DeactivatePasswordRecoveryToken(U customerId)
        {
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "UpdatePasswordRecoveryTokenStatus";

                    cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = customerId;

                    db = new Database();

                    db.UpdateQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
        }
        #endregion

    }   // class
}   // namespace
