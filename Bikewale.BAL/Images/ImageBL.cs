using Bikewale.Entities.AWS;
using Bikewale.Entities.Images;
using Bikewale.Interfaces.Images;
using Bikewale.Interfaces.Security;
using Bikewale.Notifications;
using Newtonsoft.Json;
using RabbitMqPublishing;
using System;
using System.Collections.Specialized;
using System.Linq;
using Bikewale.Entities.CMS.Photos;
using System.Collections.Generic;
using Grpc.CMS;
using Bikewale.BAL.GrpcFiles;

namespace Bikewale.BAL.Images
{
    /// <summary>
    /// Created by  :   Sumit Kate on 09 Nov 2016
    /// Description :   This class holds the bussiness logic methods related to operation of images
    /// </summary>
    public class ImageBL : IImage
    {
        private readonly string _environment = Utility.BWConfiguration.Instance.AWSEnvironment;
        private readonly string _queue = Utility.BWConfiguration.Instance.AWSImageQueueName;

        private readonly IImageRepository<Entities.Images.Image, ulong> _objDAL = null;
        private readonly ISecurity _security = null;
        public ImageBL()
        {

        }
        public ImageBL(IImageRepository<Entities.Images.Image, ulong> objDAL, ISecurity security)
        {
            _objDAL = objDAL;
            _security = security;
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 09 Nov 2016
        /// Description :   This method saves image details in database and also gets token required for further processing request
        /// Modified by :   Sumit Kate on 06 Jan 2017
        /// Description :   If Original Image path is provided pass the original image path into token else pass the key
        /// </summary>
        /// <param name="objImage"></param>
        /// <returns></returns>
        public Entities.Images.ImageToken GenerateImageUploadToken(Entities.Images.Image objImage)
        {
            ImageToken token = new ImageToken();
            Token awsToken = null;
            try
            {
                _objDAL.Add(objImage);
                if (objImage.Id.HasValue && objImage.Id.Value > 0)
                {
                    token.Id = (uint)objImage.Id.Value;
                    string hash = _security.GenerateHash((uint)objImage.Id.Value);
                    awsToken = _security.GetToken();
                    token.Key = String.Format("n/bw/{0}{1}_{2}.{3}", _environment, hash, objImage.Id, (!String.IsNullOrEmpty(objImage.Extension) ? objImage.Extension : "jpg"));
                    token.OriginalImagePath = IfNull(objImage.OriginalPath, token.Key);
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
                ErrorClass.LogError(ex, String.Format("GenerateImageUploadToken{0}", Newtonsoft.Json.JsonConvert.SerializeObject(objImage)));
            }
            return token;
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 06 Jan 2017
        /// Description :   Returns the First non-null/Empty string or null
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private String IfNull(params string[] arr)
        {
            return arr.FirstOrDefault(m => !String.IsNullOrEmpty(m));
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 10 nov 2016
        /// Description :   Sends the token to BW-IPC for further processing
        /// Modified by :   Sumit Kate on 06 Jan 2017
        /// Description :   If Original Image path is provided pass the original image path into token else pass the key
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ImageToken ProcessImageUpload(ImageToken token)
        {
            try
            {
                var hashId = token.Key.Substring(5 + _environment.Length, Utility.BWConfiguration.Instance.SecurityHashLength);
                //get hash id from the original path
                if (_security.VerifyHash(hashId, Convert.ToUInt32(token.Id)))
                {
                    NameValueCollection objNVC = new NameValueCollection();
                    objNVC.Add("id", token.Id.ToString());
                    objNVC.Add("originalPath", "/" + IfNull(token.OriginalImagePath, token.Key));
                    objNVC.Add("photoId", token.PhotoId.ToString());
                    //Process further through rabbit MQ
                    RabbitMqPublish objRMQPublish = new RabbitMqPublish();
                    token.Status = objRMQPublish.PublishToQueue(_queue, objNVC);
                }
            }
            catch (Exception ex)
            {
                token.Status = false;
                ErrorClass.LogError(ex, String.Format("ProcessImageUpload({0})", JsonConvert.SerializeObject(token)));
            }
            return token;
        }

        
    }
}
