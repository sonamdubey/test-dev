using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Entity.ImageUpload;
using Carwale.Interfaces.Classified.CarDetail;
using Carwale.Interfaces.ImageUpload;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using RabbitMqPublishing;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Carwale.BL.ImageUpload
{
    public class ImageBL : IImageBL
    {
        private readonly ICarDetailsCache _carDetailsCacheRepo;
        private readonly string _usedCarsStockImageQueueName = ConfigurationManager.AppSettings["UsedCarStockImageIPCQueue"];
        private const string _awsAccessKeyId = "AKIAIXWHZPIVBBDGRENQ";
        private const string _bucketName = "m-aeplimages";
        private const string _secretKey = "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";
        private const string _seed = "8ik3wa13ghi8jkl7mno6pqr5st4u3v2w1xyz";
        private const int _baseCount = 30;
        private const int _hashLength = 7;
        private const string _s3region = "ap-south-1";

        public ImageBL(ICarDetailsCache carDetailsCacheRepo)
        {
            _carDetailsCacheRepo = carDetailsCacheRepo;
        }
        
        public ImageTokenDTO GenerateImageUploadToken(int inquiryId, string extension, int imageType)
        {
            ImageTokenDTO token = new ImageTokenDTO();
            try
            {
                uint inqId = Convert.ToUInt32(inquiryId);
                var basicCarInfo = _carDetailsCacheRepo.GetIndividualListingDetails(inqId)?.BasicCarInfo;
                string hmacSHAKey = "awsshakeyforimageupload";

                if (basicCarInfo != null)
                {
                    string imageName = imageType == 6 ? $"rc-{new Random().Next(100000, 1000000)}-{DateTime.Now.Ticks}"
                        : $"{Format.FormatURL(basicCarInfo.MakeName)}-{Format.FormatURL(basicCarInfo.RootName)}-{Format.FormatURL(basicCarInfo.VersionName)}-{DateTime.Now.Ticks}";

                    string originalImagePath = $"cw/ucp/{basicCarInfo.ProfileId.ToLower()}/{imageName}.{(!String.IsNullOrEmpty(extension) ? extension.ToLower() : "jpg")}";
                    string hash = BitConverter.ToString(HmacSHA256(originalImagePath, Encoding.UTF8.GetBytes((hmacSHAKey).ToCharArray())));

                    var awsToken = GetToken();
                    token.Key = hash;
                    token.OriginalImgPath = originalImagePath;
                    token.Policy = awsToken.Policy;
                    token.Signature = awsToken.Signature;
                    token.AccessKeyId = awsToken.AccessKeyId;
                    token.URI = awsToken.URI;
                    token.Status = true;
                    token.DatetTmeISO = awsToken.DatetTmeISO;
                    token.DateTimeISOLong = awsToken.DateTimeISOLong;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return token;
        }

        public byte[] HmacSHA256(String data, byte[] key)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(Encoding.UTF8.GetBytes(data));
        }
        
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

                awsURI = String.Format("https://{0}.s3.amazonaws.com/", _bucketName);
                string jsonPolicy = String.Format("{{'expiration':'{0}','conditions': [{{'bucket':'{1}'}},['starts-with', '$key', ''],['starts-with', '$Content-Type', ''],{{'acl': 'public-read'}},{{'success_action_status': '201'}},{{'x-amz-algorithm': 'AWS4-HMAC-SHA256'}},{{'x-amz-credential':'{2}/{3}/{4}/s3/aws4_request'}},{{'x-amz-date': '{5}'}}]}}", DateTime.Now.AddHours(1).ToString("yyyy-MM-ddTHH:mm:sss.fffZ"), _bucketName, _awsAccessKeyId, datetimeiso, _s3region, datetimeisolong);

                policyJSONBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonPolicy));

                string secret = _secretKey;
                byte[] kSecret = Encoding.UTF8.GetBytes(("AWS4" + secret).ToCharArray());
                byte[] kDate = HmacSHA256(datetimeiso, kSecret);
                byte[] kRegion = HmacSHA256(_s3region, kDate);
                byte[] kService = HmacSHA256("s3", kRegion);
                byte[] kSigning = HmacSHA256("aws4_request", kService);
                byte[] signatureBytes = HmacSHA256(policyJSONBase64, kSigning);
                signature = BitConverter.ToString(signatureBytes).Replace("-", "").ToLower();

                objAwsToken = new Token()
                {
                    Policy = policyJSONBase64,
                    Signature = signature,
                    AccessKeyId = _awsAccessKeyId,
                    URI = awsURI,
                    DatetTmeISO = datetimeiso,
                    DateTimeISOLong = datetimeisolong
                };
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return objAwsToken;
        }

        public string GenerateHash(uint uniqueId)
        {
            string baseHash = "";
            char[] hashPadding = null;
            try
            {
                while (uniqueId != 0)
                {
                    baseHash += _seed[(int)(uniqueId % _baseCount)];
                    uniqueId /= _baseCount;
                }

                int hashLength = baseHash.Length;
                hashPadding = new char[_hashLength - hashLength];

                Random objRand = new Random();
                for (int i = 0; i < _hashLength - hashLength; i++)
                {
                    hashPadding[i] = _seed[objRand.Next(_seed.Length)];
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return (new string(hashPadding) + baseHash);
        }

        public void PushToIPCQueue(int imageId, string originalImgPath)
        {
            NameValueCollection objNVC = new NameValueCollection();
            objNVC.Add("id", imageId.ToString());
            objNVC.Add("originalPath", originalImgPath);

            RabbitMqPublish objRMQPublish = new RabbitMqPublish();
            objRMQPublish.PublishToQueue(_usedCarsStockImageQueueName, objNVC);
        }
    }
}
