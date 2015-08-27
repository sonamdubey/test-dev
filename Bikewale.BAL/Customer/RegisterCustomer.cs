﻿using Bikewale.CoreDAL;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.BAL.Customer
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 28/8/2012
    /// </summary>
    public class RegisterCustomer
    {
        #region GenerateRandomPassword Method
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
                //ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                //objErr.SendMail();
            }

            return pass;
        }   // End of GenerateRandomPassword 
        #endregion

        #region GenerateRandomSalt Method
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
                //ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                //objErr.SendMail();
            }

            return salt;
        }   // End of GenerateRandomSalt Method 
        #endregion

        #region GenerateHashCode Method
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
                //ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                //objErr.SendMail();
            }

            return hashCode;
        }   // End of GenerateHashCode 
        #endregion
    }
}
