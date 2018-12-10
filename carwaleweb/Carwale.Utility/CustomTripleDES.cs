using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Utility
{
    public class CustomTripleDES
    {
        #region Data Encryption-Decryption
        /// <summary>
        /// To encrypt the Data using Triple DES Encryption
        /// </summary>
        /// <param name="sIn">The String given as Input</param>
        /// <returns>The encrypted resultant string as output</returns>
        public static string EncryptTripleDES(string sIn)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
            // Compute the MD5 hash.
            DES.Key = hashMD5.ComputeHash(UTF8Encoding.UTF8.GetBytes("c@rW@Le"));
            //Clear resource used by MD5 encryptor
            hashMD5.Clear();
            // Set the cipher mode.
            DES.Mode = CipherMode.ECB;
            //padding mode
            DES.Padding = PaddingMode.PKCS7;
            // Create the encryptor.
            ICryptoTransform DESEncrypt = DES.CreateEncryptor();
            // Get a byte array of the string.
            byte[] Buffer = UTF8Encoding.UTF8.GetBytes(sIn);
            //get the result array
            byte[] resultArray = DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length);
            //Release the resources held by tripleDES Encryptor
            DES.Clear();
            // Transform and return the string.
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// To Decrypt the Encrpted data by Triple DES Decryption method. 
        /// </summary>
        /// <param name="sOut">The string Encrpted by Triples DES Encrption</param>
        /// <returns>Resultant Output String</returns>
        public static string DecryptTripleDES(string sOut)
        {
            string retVal = string.Empty;
            try
            {
                sOut = sOut.Replace(' ', '+');
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
                // Compute the MD5 hash.
                DES.Key = hashMD5.ComputeHash(UTF8Encoding.UTF8.GetBytes("c@rW@Le"));
                //Clear resource used by MD5 encryptor
                hashMD5.Clear();
                // Set the cipher mode.
                DES.Mode = CipherMode.ECB;
                //padding mode
                DES.Padding = PaddingMode.PKCS7;
                // Create the decryptor.
                ICryptoTransform DESDecrypt = DES.CreateDecryptor();
                byte[] Buffer = Convert.FromBase64String(sOut);
                //get the result array
                byte[] resultArray = DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length);
                //Release the resources held by tripleDES Encryptor
                DES.Clear();
                // Transform and return the string.
                retVal = UTF8Encoding.UTF8.GetString(resultArray, 0, resultArray.Length);
            }
            catch (Exception)
            {
                retVal = "-1";
            }
            return retVal;
        }
        #endregion
    }
}
