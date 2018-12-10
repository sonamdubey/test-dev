// Security Class
//

using System;
using System.Web;
using System.Configuration;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Carwale.Utility
{
    public static class CarwaleSecurity
    {
        private static readonly string _securityPassword = ConfigurationManager.AppSettings["SecurityPassword"];
        // Generate verification code.
        public static string EncodeVerificationCode(string userId)
        {
            string code = "";
            double _userId = 0;
            double m = 4977865319;
            double p = 886754;

            if (Regex.IsMatch(userId, "^[0-9]+$"))
                _userId = double.Parse(userId);

            // Formula is: code = ( userId * m ) + p

            code = (_userId * m + p).ToString();
            return code;
        }

        // Decode verification code and get userId.
        public static string DecodeVerificationCode(string code)
        {
            string userId = "";
            double _code = 0; // local variable to store code
            double m = 4977865319;
            double p = 886754;

            if (Regex.IsMatch(@"^[0-9]+$", code))
                _code = double.Parse(code);
            else
                userId = "-1";

            if (_code != 0)
            {
                // Formula is: userId = ( code - p ) / m

                userId = ((_code - p) / m).ToString();

                if (userId.IndexOf(".") >= 0)
                    userId = "-1";
            }

            return userId;
        }

        /**********************************************************************
          Functions below are obsolete and are for backward compatibility only.
         **********************************************************************/



        // Encrypt a string into a string using a password

        //    Uses Encrypt(byte[], byte[], byte[])

        public static string Encrypt(string clearText)
        {
            // First we need to turn the input string into a byte array.
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);

            // Then, we need to turn the password into Key and IV
            // We are using salt to make it harder to guess our key using a dictionary attack -
            // trying to guess a password by enumerating all possible words.
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(_securityPassword,

                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            // Now get the key/IV and do the encryption using the function that accepts byte arrays.
            // Using PasswordDeriveBytes object we are first getting 32 bytes for the Key
            // (the default Rijndael key length is 256bit = 32bytes) and then 16 bytes for the IV.
            // IV should always be the block size, which is by default 16 bytes (128 bit) for Rijndael.
            // If you are using DES/TripleDES/RC2 the block size is 8 bytes and so should be the IV size.
            // You can also read KeySize/BlockSize properties off the algorithm to find out the sizes.
            byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            // Now we need to turn the resulting byte array into a string.
            // A common mistake would be to use an Encoding class for that. It does not work
            // because not all byte values can be represented by characters.
            // We are going to be using Base64 encoding that is designed exactly for what we are
            // trying to do.

            return Convert.ToBase64String(encryptedData).Replace("+", "%2b").Replace("&", "%26").Replace("#", "%23");

        }


        // Decrypt a string into a string using a password

        //    Uses Decrypt(byte[], byte[], byte[])

        public static string Decrypt(string cipherText, bool isReplaced = false)
        {
            if (isReplaced)
            {
                cipherText = cipherText.Replace("%2b", "+").Replace("%26", "&").Replace("%23", "#");
            }
            string decodedData = "";

            // First we need to turn the input string into a byte array.
            // We presume that Base64 encoding was used
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            // Then, we need to turn the password into Key and IV
            // We are using salt to make it harder to guess our key using a dictionary attack -
            // trying to guess a password by enumerating all possible words.
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(_securityPassword,

                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            // Now get the key/IV and do the decryption using the function that accepts byte arrays.
            // Using PasswordDeriveBytes object we are first getting 32 bytes for the Key
            // (the default Rijndael key length is 256bit = 32bytes) and then 16 bytes for the IV.
            // IV should always be the block size, which is by default 16 bytes (128 bit) for Rijndael.
            // If you are using DES/TripleDES/RC2 the block size is 8 bytes and so should be the IV size.
            // You can also read KeySize/BlockSize properties off the algorithm to find out the sizes.
            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            // Now we need to turn the resulting byte array into a string.
            // A common mistake would be to use an Encoding class for that. It does not work
            // because not all byte values can be represented by characters.
            // We are going to be using Base64 encoding that is designed exactly for what we are
            // trying to do.

            decodedData = System.Text.Encoding.Unicode.GetString(decryptedData);


            return decodedData;
        }

        // Encrypt a byte array into a byte array using a key and an IV
        public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            // Create a MemoryStream that is going to accept the encrypted bytes
            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm.
            // We are going to use Rijndael because it is strong and available on all platforms.
            // You can use other algorithms, to do so substitute the next line with something like
            //                      TripleDES alg = TripleDES.Create();
            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV.
            // We need the IV (Initialization Vector) because the algorithm is operating in its default
            // mode called CBC (Cipher Block Chaining). The IV is XORed with the first block (8 byte)
            // of the data before it is encrypted, and then each encrypted block is XORed with the
            // following block of plaintext. This is done to make encryption more secure.
            // There is also a mode called ECB which does not need an IV, but it is much less secure.
            alg.Key = Key;

            alg.IV = IV;

            // Create a CryptoStream through which we are going to be pumping our data.
            // CryptoStreamMode.Write means that we are going to be writing data to the stream
            // and the output will be written in the MemoryStream we have provided.
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the encryption
            cs.Write(clearData, 0, clearData.Length);

            // Close the crypto stream (or do FlushFinalBlock).
            // This will tell it that we have done our encryption and there is no more data coming in,
            // and it is now a good time to apply the padding and finalize the encryption process.
            cs.Close();

            // Now get the encrypted data from the MemoryStream.
            // Some people make a mistake of using GetBuffer() here, which is not the right way.
            byte[] encryptedData = ms.ToArray();

            return encryptedData;

        }

        // Decrypt a byte array into a byte array using a key and an IV
        public static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            // Create a MemoryStream that is going to accept the decrypted bytes
            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm.
            // We are going to use Rijndael because it is strong and available on all platforms.
            // You can use other algorithms, to do so substitute the next line with something like
            //                      TripleDES alg = TripleDES.Create();
            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV.
            // We need the IV (Initialization Vector) because the algorithm is operating in its default
            // mode called CBC (Cipher Block Chaining). The IV is XORed with the first block (8 byte)
            // of the data after it is decrypted, and then each decrypted block is XORed with the previous
            // cipher block. This is done to make encryption more secure.
            // There is also a mode called ECB which does not need an IV, but it is much less secure.
            alg.Key = Key;

            alg.IV = IV;

            // Create a CryptoStream through which we are going to be pumping our data.
            // CryptoStreamMode.Write means that we are going to be writing data to the stream
            // and the output will be written in the MemoryStream we have provided.
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the decryption
            cs.Write(cipherData, 0, cipherData.Length);

            // Close the crypto stream (or do FlushFinalBlock).
            // This will tell it that we have done our decryption and there is no more data coming in,
            // and it is now a good time to remove the padding and finalize the decryption process.
            cs.Close();

            // Now get the decrypted data from the MemoryStream.
            // Some people make a mistake of using GetBuffer() here, which is not the right way.
            byte[] decryptedData = ms.ToArray();

            return decryptedData;

        }

        //this will return the 20 character long string generated randomly
        public static string GetRandomKey()
        {
            //generate a random password for the user
            RandomNumberGenerator rm;
            rm = RandomNumberGenerator.Create();

            string sRand = "";
            string sTmp = "";

            byte[] data = new byte[35];
            rm.GetNonZeroBytes(data);

            for (int nCnt = 0; nCnt <= data.Length - 1; nCnt++)
            {
                //First convert it into a integer
                int nVal = Convert.ToInt32(data.GetValue(nCnt));
                // Check whether the converted int falls in between alphabets, and numbers
                if ((nVal >= 48 && nVal <= 57) || (nVal >= 97 && nVal <= 122))
                {
                    sTmp = Convert.ToChar(nVal).ToString(); //Convert to character
                }
                else
                {
                    sTmp = nVal.ToString(); //Remain as integer
                }
                sRand += sTmp.ToString(); //Append it to a string

                //HttpContext.Current.Trace.Warn("nVal : " + nVal.ToString() + " : sRand : " + sRand);
            }

            //get the first 20 characters from the random string and use it as the password
            string key = sRand.Substring(0, 20);

            return key;
        }


        //simple symmetric algorithm
        public static string EncryptUsingSymmetric(string password, string valueToEncrypt)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(valueToEncrypt);

            // Then, we need to turn the password into Key and IV
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,
                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            // Now get the key/IV and do the encryption using the function that accepts byte arrays.
            // Using PasswordDeriveBytes object we are first getting 32 bytes for the Key
            // (the default Rijndael key length is 256bit = 32bytes) and then 16 bytes for the IV.
            byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            // Now we need to turn the resulting byte array into a string.
            // A common mistake would be to use an Encoding class for that. It does not work
            // because not all byte values can be represented by characters.
            // We are going to be using Base64 encoding that is designed exactly for what we are
            // trying to do.

            return Convert.ToBase64String(encryptedData);
        }


        public static string DecryptUsingSymmetric(string password, string cipherText)
        {
            string decodedData = "";


            // First we need to turn the input string into a byte array.
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            // Then, we need to turn the password into Key and IV
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            // Now get the key/IV and do the decryption using the function that accepts byte arrays.
            // Using PasswordDeriveBytes object we are first getting 32 bytes for the Key
            // (the default Rijndael key length is 256bit = 32bytes) and then 16 bytes for the IV.
            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            // Now we need to turn the resulting byte array into a string.
            // A common mistake would be to use an Encoding class for that. It does not work
            // because not all byte values can be represented by characters.
            // We are going to be using Base64 encoding that is designed exactly for what we are
            // trying to do.

            decodedData = System.Text.Encoding.Unicode.GetString(decryptedData);

            return decodedData;
        }

        public static string EncryptUserId(long userId)
        {
            string userKey = "";

            userKey = ((32605878 * userId) + 27890291).ToString();

            return userKey;
        }

        public static string DecryptUserId(long userKey)
        {
            string userId = "";

            userId = ((userKey - 27890291) / 32605878).ToString();

            return userId;
        }

        /// <summary>
        /// Used to encrypt msg string in BillDesk payment gateway
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetHMACSHA256(string text, string key)
        {
            UTF8Encoding encoder = new UTF8Encoding();

            byte[] hashValue;
            byte[] keybyt = encoder.GetBytes(key);
            byte[] message = encoder.GetBytes(text);

            HMACSHA256 hashString = new HMACSHA256(keybyt);
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

    }
}