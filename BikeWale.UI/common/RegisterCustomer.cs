using Bikewale.Notifications;
using Bikewale.Notifications.MailTemplates;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using Bikewale.Utility;

namespace Bikewale.Common
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 28/8/2012
    /// </summary>
    public class RegisterCustomer
    {

        #region RegisterUser PopulateWhere
        /// <summary>
        ///     Written By : Ashish G. Kamble on 28/8/2012
        ///     Summary : Function will register customer and return the customer id. If customer is already a registered customer returns registered id.
        ///     Modified By : Sadhana Upadhyay on 2nd April 2014
        ///     Summary : To capture Client IP.
        /// </summary>
        /// <param name="name">name of the customer. </param>
        /// <param name="email">Email of the customer. This Budget can not be empty.</param>
        /// <param name="mobile">Mobile no of the customer. This Budget can not be empty.</param>
        /// <param name="phone">Phone number of the customer is optional.</param>
        /// <param name="password">Password of the customer is optional.</param>
        /// <param name="cityId">CityId of the customer is optional.</param>
        /// <returns>Returns the customer id of the customer.</returns>
        public string RegisterUser(string name, string email, string mobile, string phone, string password, string cityId)
        {
            string val = "";
            string salt = String.Empty, hash = String.Empty;
            bool isNew = false;

            //CommonOpn op = new CommonOpn();

            string customerId = "";

            // If password is not given by customer generate random password (In case of automate registration).
            // Else use customer given password.
            // Create salt and hash for the password.
            if (String.IsNullOrEmpty(password))
            {
                password = GenerateRandomPassword();
                salt = GenerateRandomSalt();
                hash = GenerateHashCode(password, salt);
            }
            else
            {
                salt = GenerateRandomSalt();
                hash = GenerateHashCode(password, salt);
            }

            HttpContext.Current.Trace.Warn("password : " + password);
            HttpContext.Current.Trace.Warn("salt : " + salt);
            HttpContext.Current.Trace.Warn("hash : " + hash);

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("registercustomer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbType.String, 50, name));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 50, email.Trim().ToLower()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 20, String.IsNullOrEmpty(mobile) ? Convert.DBNull : mobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, String.IsNullOrEmpty(cityId) ? Convert.DBNull : cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_phoneno", DbType.String, 20, String.IsNullOrEmpty(phone) ? Convert.DBNull : phone));
                    //cmd.Parameters.Add(DbFactory.GetDbParam("par_password", DbType.String, 20,String.IsNullOrEmpty(password) ? Convert.DBNull : password));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_salt", DbType.String, 10, salt));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hash", DbType.String, 64, hash));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isnew", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, CommonOpn.GetClientIP()));

                    //Bikewale.Notifications.// LogLiveSps.LogSpInGrayLog(cmd);

                    //run the command

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    customerId = cmd.Parameters["par_customerid"].Value.ToString();

                    // Send confirmation email for first time registration
                    // IsNew = 1 
                    isNew = Convert.ToBoolean(cmd.Parameters["par_isnew"].Value);

                    // Send email to the customer
                    if (!String.IsNullOrEmpty(customerId) && isNew)
                    {
                        Common.Mails.CustomerRegistration(customerId, password);
                        HttpContext.Current.Trace.Warn("Register Customer done. Mail sent to customer.");
                    }
                    HttpContext.Current.Trace.Warn("CustomerId : " + customerId);
                }
            }
            catch (SqlException err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                HttpContext.Current.Trace.Warn(err.Message);
                Exception ex = new Exception(err.Message + "Values : " + val);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            } // catch SqlException
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                Exception ex = new Exception(err.Message + "Values : " + val);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            } // catch Exception

            return customerId;
        }
        #endregion

        #region IsRegisterdCustomer PopulateWhere
        /// <summary>
        ///     Summary : Function to check whether customer is registered user or not.
        ///     Modified By : Ashish G. Kamble on 8 Nov 2012
        /// </summary>
        /// <param name="Email">Email id given by customer</param>
        /// <returns>Returns customer id. If customer is registered returns customerid else returns -1.</returns>
        public string IsRegisterdCustomer(string Email)
        {
            string cust_id = string.Empty;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getcustomerid"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = Email;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 50, Email));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null && dr.Read())
                        {
                            cust_id = dr["ID"].ToString();
                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException exSql)
            {
                ErrorClass.LogError(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return cust_id;
        }
        #endregion

        #region GenerateRandomPassword PopulateWhere
        /// <summary>
        ///     Written By : Ashish G.Kamble on 29 Oct 2012
        ///     Summary : Function will generate the random password. This function will be used during automate customer registration.
        /// </summary>
        /// <returns>Function returns randomly generated 8 digit password</returns>
        public string GenerateRandomPassword()
        {
            string pass = string.Empty;
            int pwdSize = 8;

            try
            {
                PasswordHashingLib.PasswordHashing objPass = new PasswordHashingLib.PasswordHashing();
                pass = objPass.GenerateRandomSalt(pwdSize);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return pass;
        }   // End of GenerateRandomPassword 
        #endregion

        #region GenerateRandomSalt PopulateWhere
        /// <summary>
        ///     Written By : Ashish G. Kamble on 29 Oct 2012
        ///     Summary : Function will create the salt for hashing the password.
        ///               Salt is a uniquely generated random string to create hash. Its a key to create the hash.
        ///               Salt will be of 10 digits.
        /// </summary>
        /// <returns>Function return randomly generated salt.</returns>
        public string GenerateRandomSalt()
        {
            string salt = string.Empty;
            int saltSize = 10;

            try
            {
                PasswordHashingLib.PasswordHashing objPass = new PasswordHashingLib.PasswordHashing();
                salt = objPass.GenerateRandomSalt(saltSize);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return salt;
        }   // End of GenerateRandomSalt PopulateWhere 
        #endregion

        #region GenerateHashCode PopulateWhere
        /// <summary>
        ///     Written By : Ashish G.Kamble on 29 Oct 2012
        ///     Summary : Function will generate random hash code from the given salt and password.
        ///               Hash code is a randomly generated unique string for the given password.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns>Function will return the hash code</returns>
        public string GenerateHashCode(string password, string salt)
        {
            string hashCode = string.Empty;

            try
            {
                if (!String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(salt))
                {
                    PasswordHashingLib.PasswordHashing objPass = new PasswordHashingLib.PasswordHashing();
                    hashCode = objPass.ComputePasswordHash(password, salt);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return hashCode;
        }   // End of GenerateHashCode 
        #endregion

        #region IsValidPassword PopulateWhere
        /// <summary>
        ///     Written By : Ashish G. Kamble on 29 Oct 2012
        ///     Summary : Function will check whether the user given password matches the password for the given user in database or not.
        /// </summary>
        /// <param name="password">Its user given password</param>
        /// <param name="email">Provide Customer's email id whose password need to be validated.</param>
        /// <returns></returns>
        public Customers IsValidPassword(string password, string email)
        {
            string salt = string.Empty, hash = string.Empty, userHash = string.Empty, customerId = string.Empty, name = string.Empty;
            //bool isValidUser = false;
            Customers objCust = null;

            // Get salt and hash code from database for the customer
            customerId = GetSaltHash(ref salt, ref hash, ref customerId, ref name, email);

            // Generate hash for the user given password
            userHash = GenerateHashCode(password, salt);
            HttpContext.Current.Trace.Warn("user computed hash : " + userHash);

            objCust = new Customers();
            // Check if generated hash is same as stored in the database.
            if (hash.Equals(userHash))
            {
                //isValidUser = true;                
                objCust.Email = email;
                objCust.Id = customerId;
                objCust.Name = name;
            }

            return objCust;
        }   // End of IsValidPassword 
        #endregion

        #region GetSaltHash PopulateWhere
        /// <summary>
        ///     Written By : Ashish G. Kamble on 29 Oct 2012
        ///     Summary : Function will return the salt, hash and customerid stored in the database for the given email id
        /// </summary>
        /// <param name="salt">Salt from database will be return in the supplied parameter</param>
        /// <param name="hash">Hash from database will be return in the supplied parameter</param>
        /// <param name="email">Provide email id of a customer whose salt and hash is required</param>
        public string GetSaltHash(ref string salt, ref string hash, ref string customerId, ref string name, string email)
        {
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getcustomerpassword";

                    //cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = email;
                    //cmd.Parameters.Add("@salt", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("@Hash", SqlDbType.VarChar, 64).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("@customerid", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    // Bikewale.Notifications.// LogLiveSps.LogSpInGrayLog(cmd);
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 100, email));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hash", DbType.String, 64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_salt", DbType.String, 10, ParameterDirection.Output));


                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);

                    salt = cmd.Parameters["par_salt"].Value.ToString();
                    hash = cmd.Parameters["par_hash"].Value.ToString();
                    customerId = cmd.Parameters["par_customerid"].Value.ToString();
                    name = cmd.Parameters["par_name"].Value.ToString();
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

            return customerId;
        }   // End of GetSaltHash method 
        #endregion

        #region UpdatePassword PopulateWhere
        /// <summary>
        ///     Written By : Ashish G. Kamble on 29 Oct 2012
        ///     Summary : function will update the salt and hash for given customerid.
        /// </summary>
        /// <param name="salt">New salt to be updated</param>
        /// <param name="hash">New hash generated from password and salt</param>
        /// <param name="customerId">Id of the customer whose salt and hash need to be updated</param>
        public void UpdatePassword(string salt, string hash, string customerId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "updatecustomerpassword";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_salt", DbType.String, 10, salt));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hash", DbType.String, 64, hash));

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

        }   // End of UpdatePassword method 
        #endregion

        #region EncryptPasswordToken PopulateWhere
        /// <summary>
        ///     Written By : Ashish G. Kamble on 5 Nov 2012
        ///     Summary : Function will create alphanumeric token from email id.
        /// </summary>
        /// <returns>Returns randomly generated token</returns>
        public string EncryptPasswordToken(string value)
        {
            string token = string.Empty;

            token = Bikewale.Utility.TripleDES.EncryptTripleDES(value);

            return token;
        }
        #endregion

        #region DecryptPasswordToken PopulateWhere
        /// <summary>
        ///     Written By : Ashish G. Kamble on 6 Nov 2012
        ///     Summary : Function will decrypt the token to give original value
        /// </summary>
        /// <param name="token">String encrypted by EncryptPasowordToken method</param>
        /// <returns>Returns original string from token</returns>
        public string DecryptPasswordToken(string token)
        {
            string decodedValue = string.Empty;
            HttpContext.Current.Trace.Warn("passed token : " + token);
            decodedValue = Bikewale.Utility.TripleDES.DecryptTripleDES(token);
            HttpContext.Current.Trace.Warn("decoded token : " + decodedValue);

            return decodedValue;
        }
        #endregion

        #region SaveToken PopulateWhere
        /// <summary>
        ///     Written By : Ashish G. Kamble on 5 Nov 2012
        ///     Summary : Function will save the token into the database for current userId.
        /// </summary>
        /// <param name="customerId">Id of the customer for which token is to be saved.</param>
        /// <returns>returns the token (alphanumeric)</returns>
        public string SaveToken(string customerId, string email)
        {
            string token = string.Empty;
            try
            {
                token = EncryptPasswordToken(email);

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

            return token;
        }   // End of SaveToken method 
        #endregion

        #region IsValidPasswordRecoveryToken PopulateWhere
        /// <summary>
        ///     Written By : Ashish G. Kamble on 6 Nov 2012
        ///     Summary : Function will check whether the token is valid for the current user or not.
        ///               Password recovery token is valid for 24 hours only.
        /// </summary>
        /// <param name="customerId">id of the customer whose token is tob validated</param>
        /// <param name="token">token given given by customer</param>
        /// <returns>Returns true or false if token is valid</returns>
        public bool IsValidPasswordRecoveryToken(string customerId, string token)
        {
            bool isValidtoken = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "checkvalidpasswordtoken";

                    //cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = customerId;
                    //cmd.Parameters.Add("@Token", SqlDbType.VarChar, 200).Value = token;
                    //cmd.Parameters.Add("@isvalidtoken", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    //Bikewale.Notifications.// LogLiveSps.LogSpInGrayLog(cmd);
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_token", DbType.String, 200, token));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isvalidtoken", DbType.Boolean, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);

                    isValidtoken = Convert.ToBoolean(cmd.Parameters["par_isvalidtoken"].Value);
                    HttpContext.Current.Trace.Warn("isValidtoken : " + isValidtoken.ToString());
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
        }   // End of IsValidPasswordRecoveryToken method 
        #endregion

        #region UpdatePasswordRecoveryTokenStatus PopulateWhere
        /// <summary>
        ///     Written By : Ashish G. Kamble on 6 Nov 2012
        ///     Summary : Function will update the status of the token to inactive for the given customer.
        /// </summary>
        /// <param name="customerId">Customer id of the customer whose password recovery token is to make inactive</param>
        public void UpdatePasswordRecoveryTokenStatus(string customerId)
        {
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "updatepasswordrecoverytokenstatus";

                    //cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = customerId;
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
        }   // End of UpdatePasswordRecoveryTokenStatus method 
        #endregion

        #region SendCustomerPassword PopulateWhere
        /// <summary>
        ///     Written By : Ashish G. Kamble on 2 Nov 2012
        ///     Summary : Function will send customer an email with link to recover the password.
        /// </summary>
        /// <param name="email">Email id on which password recovery link need to be send</param>
        /// <returns>Returns true or false if mail is send or not</returns>
        public bool SendCustomerPassword(string email)
        {
            string customerId = string.Empty, token = string.Empty, custName = string.Empty;
            bool isSend = false;

            try
            {
                customerId = IsRegisterdCustomer(email);

                if (!String.IsNullOrEmpty(customerId) && customerId != "-1")
                {
                    token = SaveToken(customerId, email);

                    CustomerDetails objCustDetails = new CustomerDetails(customerId);
                    custName = objCustDetails.Name;

                    // Send Reset password link to the customer   

                    ComposeEmailBase objEmail = new PasswordRecoveryMail(custName, email, token);
                    objEmail.Send(email, "BikeWale Password Recovery");

                    isSend = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return isSend;
        }   // End of SendCustomerPassword method 
        #endregion

        #region SendCustomerPassword Old PopulateWhere Commented By : Ashish G. Kamble
        //public bool SendCustomerPassword(string email)
        //{
        //    bool retVal = false;

        //    email = email.Trim();
        //    string userId = "", name = "", password = "", salt = string.Empty, hash = string.Empty;

        //    Database db = new Database();
        //    SqlDataReader reader = null;
        //    try
        //    {
        //        //string sql = "SELECT Id, Name, password FROM Customers WHERE Isfake = 0 AND email=@email";
        //        string sql = "SELECT Id, Name FROM Customers WHERE Isfake = 0 AND email=@email";

        //        SqlParameter[] param = { new SqlParameter("@email", email) };

        //        reader = db.SelectQry(sql, param);

        //        if (reader.Read())
        //        {
        //            userId = reader["Id"].ToString();
        //            name = reader["Name"].ToString();
        //            //password = reader["Password"].ToString();

        //            GenerateRandomPassword();
        //            salt = GenerateRandomSalt();
        //            hash = GenerateHashCode(password, salt);

        //            UpdatePassword(salt, hash, userId);
        //        }

        //        if (userId == "") retVal = false;
        //        else
        //        {
        //            Bikewale.Common.Mails.CustomerPasswordRecovery(email, name, password);
        //            retVal = true;
        //        }
        //    }
        //    catch (SqlException err)
        //    {
        //        //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
        //        ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
        //        
        //    } // catch SqlException
        //    catch (Exception err)
        //    {
        //        ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
        //        
        //    } // catch Exception
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //        }
        //        db.CloseConnection();
        //    }
        //    return retVal;
        //}   // End of SendCustomerPassword method 
        #endregion


        /// <summary>
        /// Written By : Ashwini Todkar on 19 feb 2014
        /// summary    : method update customer mobile number and name if changed
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mobile"></param>
        public void UpdateCustomerMobile(string mobile, string email, string name = null)
        {
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("updatecustomermobile"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 20).Value = mobile;
                    //cmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email;
                    //if (!String.IsNullOrEmpty(name)) { cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = name; }

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 20, mobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 100, email));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbType.String, 50, (!String.IsNullOrEmpty(name)) ? name : Convert.DBNull));

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

        /// <summary>
        /// Created By : Sadhana Upadhyay on 13 Oct 2014
        /// Summary : To get isfake flag by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool IsFakeCustomer(int customerId)
        {
            bool isFake = false;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "checkfakecustomerbyid";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int32, customerId));

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

            return isFake;
        }   //End of IsFakeCustomer PopulateWhere
    }   // End of class
}   // End of namespace