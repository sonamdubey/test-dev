using System;
using System.Security.Cryptography;
using System.Text;

namespace Carwale.Utility
{
    public class AESEncryptionUtility
    {
        public static byte[] Encrypt(byte[] plainBytes, byte[] key, byte[] IV, CipherMode cipherMode, PaddingMode padding)
        {
            byte[] encrypted = null;
            using (AesCryptoServiceProvider aesAlgo = GetAesCryptoServiceProvider(key, IV, cipherMode, padding))
            {
                ICryptoTransform encryptor = aesAlgo.CreateEncryptor();
                encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            }
            return encrypted;
        }

        public static string EncryptUrlSafe(byte[] plainText, byte[] key, byte[] IV, CipherMode cipherMode, PaddingMode padding) // This will return safe Base64 Url Encoded string
        {
            byte[] encrypted = Encrypt(plainText, key, IV, cipherMode, padding);
            return Convert.ToBase64String(encrypted).Replace('+', '-').Replace('/', '_').Replace('=', ',');
        }

        public static string EncryptSelectorSafe(byte[] plainText, byte[] key, byte[] IV, CipherMode cipherMode, PaddingMode padding) // This will return encoded string safe for using selectors in javascript.
        {
            byte[] encrypted = Encrypt(plainText, key, IV, cipherMode, padding);
            return Convert.ToBase64String(encrypted).Replace('+', '-').Replace('/', '_').Replace('=', '.');
        }

        private static AesCryptoServiceProvider GetAesCryptoServiceProvider(byte[] key, byte[] IV, CipherMode cipherMode, PaddingMode padding)
        {
            AesCryptoServiceProvider aesAlgo = new AesCryptoServiceProvider();
            aesAlgo.Key = key;
            aesAlgo.IV = IV;
            aesAlgo.Mode = cipherMode;
            aesAlgo.Padding = padding;
            return aesAlgo;
        }

        public static byte[] Decrypt(byte[] cipherBytes, byte[] key, byte[] IV, CipherMode cipherMode, PaddingMode padding)
        {
            byte[] decrypted = null;
            using (AesCryptoServiceProvider aesAlgo = GetAesCryptoServiceProvider(key, IV, cipherMode, padding))
            {
                ICryptoTransform decryptor = aesAlgo.CreateDecryptor();
                decrypted = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            }
            return decrypted;
        }

        public static byte[] GenerateKey(string masterPassword, int size) //size in bytes
        {
            byte[] salt = GenerateSalt(size);
            Rfc2898DeriveBytes pbfdk = new Rfc2898DeriveBytes(masterPassword, salt, 20000);
            return pbfdk.GetBytes(size);
        }

        public static byte[] GenerateSalt(int size) //size in bytes
        {
            RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider();
            byte[] salt = new byte[size];
            generator.GetNonZeroBytes(salt);
            return salt;
        } 
    }
}
