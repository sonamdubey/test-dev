using Bikewale.Entities.AWS;
using Bikewale.Interfaces.Security;
using Bikewale.Notifications;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Bikewale.BAL.Security
{
    /// <summary>
    /// Created by  :   Sumit Kate on 09 Nov 2016
    /// Description :   This class holds the logic related to security.
    /// </summary>
    public class SecurityBL : ISecurity
    {
        protected string signature;
        protected string policyJSONBase64;
        private readonly string awsAccessKeyId = Utility.BWConfiguration.Instance.AWSAccessKey;
        private readonly string bucketName = Utility.BWConfiguration.Instance.AWSBucketName;
        private readonly string secretKey = Utility.BWConfiguration.Instance.AWSSecretKey;
        private const string SEED = "8ik3wa13ghi8jkl7mno6pqr5st4u3v2w1xyz";
        private const int baseCount = 30;
        private readonly int HashLength = Utility.BWConfiguration.Instance.SecurityHashLength;
        private readonly string s3region = Utility.BWConfiguration.Instance.AWSS3Region;

        /// <summary>
        /// Created by  :   Sumit Kate on 09 Nov 2016
        /// Description :   This method gets token with AWS URI to upload an image to amazon S3
        /// </summary>
        /// <returns></returns>
        public Token GetToken()
        {
            Token objAwsToken = null;
            try
            {
                string[] tokens = new string[2];
                string signature = "", policyJSONBase64 = "", awsURI = "";
                DateTime now = DateTime.Now;

                string datetimeiso = now.ToString("yyyyMMdd");
                string datetimeisolong = now.ToString("yyyyMMddTHHmmssZ");

                awsURI = String.Format("https://{0}.s3.amazonaws.com/", bucketName);
                string jsonPolicy = String.Format("{{'expiration':'{0}','conditions': [{{'bucket':'{1}'}},['starts-with', '$key', ''],['starts-with', '$Content-Type', ''],{{'acl': 'public-read'}},{{'success_action_status': '201'}},{{'x-amz-algorithm': 'AWS4-HMAC-SHA256'}},{{'x-amz-credential':'{2}/{3}/{4}/s3/aws4_request'}},{{'x-amz-date': '{5}'}}]}}", DateTime.Now.AddHours(1).ToString("yyyy-MM-ddTHH:mm:sss.fffZ"), bucketName, awsAccessKeyId, datetimeiso, s3region, datetimeisolong);

                policyJSONBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonPolicy));

                string secret = secretKey;
                byte[] kSecret = Encoding.UTF8.GetBytes(("AWS4" + secret).ToCharArray());
                byte[] kDate = HmacSHA256(datetimeiso, kSecret);
                byte[] kRegion = HmacSHA256(s3region, kDate);
                byte[] kService = HmacSHA256("s3", kRegion);
                byte[] kSigning = HmacSHA256("aws4_request", kService);
                byte[] signatureBytes = HmacSHA256(policyJSONBase64, kSigning);
                signature = BitConverter.ToString(signatureBytes).Replace("-", "").ToLower();

                objAwsToken = new Token()
                {
                    Policy = policyJSONBase64,
                    Signature = signature,
                    AccessKeyId = awsAccessKeyId,
                    URI = awsURI,
                    DatetTmeISO = datetimeiso,
                    DateTimeISOLong = datetimeisolong
                };
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SecurityBL.GetToken");
                
            }
            return objAwsToken;
        }

        static byte[] HmacSHA256(String data, byte[] key)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 09 Nov 2016
        /// Description :   To generate hash from passed unique id
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public string GenerateHash(uint uniqueId)
        {
            string baseHash = "";
            char[] hashPadding = null;
            try
            {
                while (uniqueId != 0)
                {
                    baseHash += SEED[(int)(uniqueId % baseCount)];
                    uniqueId /= baseCount;
                }

                int hashLength = baseHash.Length;
                hashPadding = new char[HashLength - hashLength];

                Random objRand = new Random();
                for (int i = 0; i < HashLength - hashLength; i++)
                {
                    hashPadding[i] = SEED[objRand.Next(SEED.Length)];
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SecurityBL.GenerateHash : " + uniqueId);
                
            }
            return (new string(hashPadding) + baseHash);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 09 Nov 2016
        /// Description :   To validate the hashValue
        /// </summary>
        /// <param name="hashValue"></param>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public bool VerifyHash(string hashValue, uint uniqueId)
        {
            bool isEqual = false;
            string newHash = "";

            try
            {
                while (uniqueId != 0)
                {
                    newHash += SEED[(int)(uniqueId % baseCount)];
                    uniqueId /= baseCount;
                }
                if (newHash == hashValue.Substring(hashValue.Length - newHash.Length))
                    isEqual = true;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "VerifyHash : " + hashValue);
                
            }
            return isEqual;
        }
    }
}
