using Bikewale.Notifications;
using BikewaleOpr.Entities.AWS;
using BikewaleOpr.Interface.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.BAL.Security
{
    /// <summary>
    /// Created by  :   Sumit Kate on 09 Nov 2016
    /// Description :   This class holds the logic related to security.
    /// </summary>
    public class SecurityBL : ISecurity
    {
        private readonly string awsAccessKeyId = Bikewale.Utility.BWOprConfiguration.Instance.AWSAccessKey;
        private readonly string bucketName = Bikewale.Utility.BWOprConfiguration.Instance.AWSBucketName;
        private readonly string secretKey = Bikewale.Utility.BWOprConfiguration.Instance.AWSSecretKey;
        private const string SEED = "8ik3wa13ghi8jkl7mno6pqr5st4u3v2w1xyz";
        private const int baseCount = 30;
        private readonly int HashLength = Bikewale.Utility.BWOprConfiguration.Instance.SecurityHashLength;

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
                awsURI = String.Format("https://{0}.s3.amazonaws.com/", bucketName);
                string jsonPolicy = String.Format("{{'expiration':'{0}','conditions': [{{'bucket':'{1}'}},['starts-with', '$key', ''],['starts-with', '$Content-Type', ''],{{'acl': 'public-read'}},{{'success_action_status': '201'}}]}}", DateTime.Now.AddHours(1).ToString("yyyy-MM-ddTHH:mm:sss.fffZ"), bucketName);

                policyJSONBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonPolicy));

                var encoding = new ASCIIEncoding();
                byte[] secretKeyBytes = encoding.GetBytes(secretKey);
                byte[] base64PolicyBytes = encoding.GetBytes(policyJSONBase64);
                var hmacsha1 = new HMACSHA1(secretKeyBytes);
                byte[] signatureBytes = hmacsha1.ComputeHash(base64PolicyBytes);
                signature = Convert.ToBase64String(signatureBytes);

                objAwsToken = new Token() { Policy = policyJSONBase64, Signature = signature, AccessKeyId = awsAccessKeyId, URI = awsURI };
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "SecurityBL.GetToken");
            }
            return objAwsToken;
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
                ErrorClass objErr = new ErrorClass(ex, "SecurityBL.GenerateHash : " + uniqueId);
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, "VerifyHash : " + hashValue);
                objErr.SendMail();
            }
            return isEqual;
        }
    }
}
