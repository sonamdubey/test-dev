using Bikewale.Utility;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace AmpCacheRefreshLibrary
{
    /// <summary>
    /// Author : Sangram Nandkhile on 08 Dec 2017
    /// Summary: Utility to refresh BikeWale amp urls
    /// </summary>
    public static class GoogleAmpCacheRefreshCall
    {
        public static bool UpdateAmpCache(string url, string privateKeyPath)
        {
            try
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var currentTimestamp = Convert.ToInt64((DateTime.Now.ToUniversalTime() - epoch).TotalSeconds);
                string urlString = string.Format("https://{0}.cdn.ampproject.org/update-cache/c/s/{1}?amp_action=flush&amp_ts={2}", BWConfiguration.Instance.AMPProjectUrl, url, currentTimestamp);
                Uri ampCacheUpdateUrl = new Uri(urlString);
                var urlSignature = SignData(ampCacheUpdateUrl.PathAndQuery, privateKeyPath);
                var queryString = HttpUtility.ParseQueryString(ampCacheUpdateUrl.Query);
                queryString.Add("amp_url_signature", urlSignature);
                var ampCacheUpdateUrlBuilder = new UriBuilder(ampCacheUpdateUrl);
                ampCacheUpdateUrlBuilder.Query = queryString.ToString();
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadString(ampCacheUpdateUrlBuilder.ToString());
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string SignData(string message, string privateKeyPath)
        {
            try
            {
                AsymmetricCipherKeyPair keyPair;
                using (var reader = File.OpenText(privateKeyPath))
                {
                    keyPair = (AsymmetricCipherKeyPair)new PemReader(reader).ReadObject();
                }
                ISigner signer = SignerUtilities.GetSigner("SHA256WithRSAEncryption");
                var encoder = new UTF8Encoding();
                byte[] originalData = encoder.GetBytes(message);
                signer.Init(true, keyPair.Private);
                signer.BlockUpdate(originalData, 0, originalData.Length);
                byte[] signedBytes = signer.GenerateSignature();

                // Convert the a base64 string before returning
                return Base64UrlEncoder.Encode(signedBytes);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}