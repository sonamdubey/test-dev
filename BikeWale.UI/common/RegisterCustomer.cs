using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Net;

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

            Database db = new Database();
            //CommonOpn op = new CommonOpn();

            string customerId = "";
            string conStr = db.GetConString();

            SqlConnection con = new SqlConnection(conStr);

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
                HttpContext.Current.Trace.Warn("Submitting Data");
                SqlCommand cmd = new SqlCommand("RegisterCustomer", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = name;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = email.Trim().ToLower();
                cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 20).Value = String.IsNullOrEmpty(mobile) ? Convert.DBNull : mobile;
                cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = String.IsNullOrEmpty(cityId) ? Convert.DBNull : cityId;
                cmd.Parameters.Add("@PhoneNo", SqlDbType.VarChar, 20).Value = String.IsNullOrEmpty(phone) ? Convert.DBNull : phone;
                //cmd.Parameters.Add("@Password", SqlDbType.VarChar, 20).Value = String.IsNullOrEmpty(password) ? Convert.DBNull : password;
                cmd.Parameters.Add("@Salt", SqlDbType.VarChar, 10).Value = salt;
                cmd.Parameters.Add("@Hash", SqlDbType.VarChar, 64).Value = hash;
                cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@IsNew", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ClientIP", SqlDbType.VarChar, 40).Value = CommonOpn.GetClientIP();
                con.Open();
                //run the command
                cmd.ExecuteNonQuery();

                customerId = cmd.Parameters["@CustomerId"].Value.ToString();
                
                // Send confirmation email for first time registration
                // IsNew = 1 
                isNew = Convert.ToBoolean(cmd.Parameters["@IsNew"].Value);

                // Send email to the customer
                if (!String.IsNullOrEmpty(customerId) && isNew)
                {
                    Common.Mails.CustomerRegistration(customerId, password);
                    HttpContext.Current.Trace.Warn("Register Customer done. Mail sent to customer.");
                }
                HttpContext.Current.Trace.Warn("CustomerId : " + customerId);
            }
            catch (SqlException err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                HttpContext.Current.Trace.Warn(err.Message);
                Exception ex = new Exception(err.Message + "Values : " + val);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch SqlException
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                Exception ex = new Exception(err.Message + "Values : " + val);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            finally
            {
                //close the connection	
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
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
            Database db = null;
            SqlDataReader dr = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand("GetCustomerId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = Email;

                    dr = db.SelectQry(cmd);
                    if (dr.Read())
                    {
                        cust_id = dr["ID"].ToString();
                    }
                }
            }
            catch (SqlException exSql)
            {
                ErrorClass objErr = new ErrorClass(exSql, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (dr != null)
                    dr.Close();
                db.CloseConnection();
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                PasswordHashingLib.PasswordHashing objPass = new PasswordHashingLib.PasswordHashing();
                hashCode = objPass.ComputePasswordHash(password, salt);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
            Database db = null;
            SqlConnection conn = null;

            try
            {
                db = new Database();

                conn = new SqlConnection(db.GetConString());

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetCustomerPassword";
                    cmd.Connection = conn;
                    HttpContext.Current.Trace.Warn("GetSaltHash email : " + email);

                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = email;
                    cmd.Parameters.Add("@Salt", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Hash", SqlDbType.VarChar, 64).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    salt = cmd.Parameters["@Salt"].Value.ToString();
                    hash = cmd.Parameters["@Hash"].Value.ToString();
                    customerId = cmd.Parameters["@CustomerId"].Value.ToString();
                    name = cmd.Parameters["@Name"].Value.ToString();

                    HttpContext.Current.Trace.Warn("GetSaltHash salt : " + salt);
                    HttpContext.Current.Trace.Warn("GetSaltHash hash : " + hash);
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
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                db.CloseConnection();
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
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "UpdateCustomerPassword";

                    cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = customerId;
                    cmd.Parameters.Add("@Salt", SqlDbType.VarChar, 10).Value = salt;
                    cmd.Parameters.Add("@Hash", SqlDbType.VarChar, 64).Value = hash;

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

            token = Utils.Utils.EncryptTripleDES(value);

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
            decodedValue = Utils.Utils.DecryptTripleDES(token);
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
            Database db = null;

            try
            {
                token = EncryptPasswordToken(email);

                db = new Database();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SavePasswordRecoveryToken";

                    cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = customerId;
                    cmd.Parameters.Add("@Token", SqlDbType.VarChar, 200).Value = token;

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
            Database db = null;
            SqlConnection conn = null;

            try
            {
                db = new Database();
                conn = new SqlConnection(db.GetConString());

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
                    HttpContext.Current.Trace.Warn("isValidtoken : " + isValidtoken.ToString());
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

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
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
            Database db = null;
            HttpContext.Current.Trace.Warn("UpdatePasswordRecoveryTokenStatus started... " + customerId);
            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "UpdatePasswordRecoveryTokenStatus";

                    cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = customerId;

                    db.UpdateQry(cmd);

                    HttpContext.Current.Trace.Warn("UpdatePasswordRecoveryTokenStatus done...");
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
                    Bikewale.Common.Mails.CustomerPasswordRecovery(email, custName, token);
                    isSend = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
        //        ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    } // catch SqlException
        //    catch (Exception err)
        //    {
        //        ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.SendMail();
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
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 13 Oct 2014
        /// Summary : To get isfake flag by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool IsFakeCustomer(int customerId)
        {
            bool isFake =false;
            Database db = null;
            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "CheckFakeCustomerById";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;

                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr.Read())
                            isFake = Convert.ToBoolean(dr["IsFake"]);
                    }
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
            return isFake;
        }   //End of IsFakeCustomer PopulateWhere
    }   // End of class
}   // End of namespace