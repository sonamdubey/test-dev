using Bikewale.Notifications;
using System;
using System.Web;

namespace Bikewale.BAL.Customer
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 28/8/2012
    /// </summary>
    public class RegisterCustomer
    {
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
                if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(salt))
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
    }
}
