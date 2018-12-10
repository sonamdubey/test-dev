using System;
using System.Configuration;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AEPLCore.Logging;

namespace Carwale.Utility
{
	public static class AwsUtils
	{
		private static IAmazonS3 client;
		private static readonly string bucketName = ConfigurationManager.AppSettings["ConfigBucketName"] != null ? ConfigurationManager.AppSettings["ConfigBucketName"] : "";
		private static Logger logger = LoggerFactory.GetLogger(typeof(AwsUtils));
		static AwsUtils()
		{
			try
			{
				if (bool.Parse(ConfigurationManager.AppSettings["IsDevelopmentEnv"] ?? "false"))
				{
					string awsAccessKeyId = ConfigurationManager.AppSettings["awsAccessKeyIdForConfig"];
					string awsSecretAccessKey = ConfigurationManager.AppSettings["awsSecretAccessKeyForConfig"];
					client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.APSouth1);
				}
				else
				{
					client = new AmazonS3Client();
				}
			}
			catch (Exception ex)
			{
				logger.LogError("error creating S3 client", ex);
			}
		}

		public static string GetObjectFromS3(string KeyName)
		{
			try
			{
				GetObjectRequest getObjectRequest = new GetObjectRequest
				{
					BucketName = bucketName,
					Key = KeyName
				};

                using (var response = client.GetObject(getObjectRequest))
                {
                    if (response != null && response.ResponseStream != null)
                    {
                        using (var stream = response.ResponseStream)
                        using (var reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        logger.LogError("null response returned from S3 client for key: " + KeyName);
                    }
                }
			}
			catch (AmazonS3Exception amazonS3Exception)
			{
				if (amazonS3Exception.ErrorCode != null &&
					(amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
					||
					amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
				{
					logger.LogError("Check the provided AWS Credentials. Access is Denied", amazonS3Exception);
				}
				else
				{
					logger.LogError("Error occurred. Message:"+ amazonS3Exception.Message + " when reading an object ", amazonS3Exception);
				}
			}
            catch (Exception ex)
            {
                logger.LogException(ex);
            }
			return null;
		}
	}
}