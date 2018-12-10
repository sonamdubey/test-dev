using Carwale.Entity.Customers;
using Carwale.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Carwale.BL.Customers
{
    public class CustomerSecurity
    {
        public static string GetRandomKey()
        {
            //generate a random password for the user
            var rm = RandomNumberGenerator.Create();

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

        public static string GetPassword()
        {
            //generate a random password for the user
            RandomNumberGenerator rm;
            rm = RandomNumberGenerator.Create();

            string sRand = "";
            string sTmp = "";

            byte[] data = new byte[15];
            rm.GetNonZeroBytes(data);

            for (int nCnt = 0; nCnt <= data.Length - 1; nCnt++)
            {
                //First convert it into a integer
                int nVal = Convert.ToInt32(data.GetValue(nCnt));
                // Check whether the converted int falls in between alphabets, and numbers
                if ((nVal >= 48 && nVal <= 57) || (nVal >= 64 && nVal <= 90) || (nVal >= 97 && nVal <= 122))
                {
                    sTmp = Convert.ToChar(nVal).ToString(); //Convert to character
                }
                else
                {
                    sTmp = nVal.ToString(); //Remain as integer
                }
                sRand += sTmp.ToString(); //Append it to a string
            }

            //get the first 6 characters from the random string and use it as the password
            string passwd = sRand.Substring(0, 6);

            return passwd.ToLower();
        }

        /// <summary>
        /// Use this to generate strings and use it as access tokens
        /// </summary>
        /// <param name="size">size of the output string</param>
        /// <returns>random string of the input size</returns>
        public static string getAccessToken(int size)
        {
            byte[] randBytes = new byte[size];
            new RNGCryptoServiceProvider().GetBytes(randBytes);
            return HttpServerUtility.UrlTokenEncode(randBytes);
        }

        public static string getRandomString(int size)
        {
            byte[] randBytes = new byte[size];
            new RNGCryptoServiceProvider().GetBytes(randBytes);
            return Convert.ToBase64String(randBytes);
        }
        /// <summary>
        /// Validates Access token n returns USER info
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static GoogleUserInfo googleTokenValidate(string accessToken)
        {
            GoogleUserInfo resp = new GoogleUserInfo();
            try
            {
                string validateURL = "https://www.googleapis.com/userinfo/v2/me?access_token=" + accessToken;
                WebRequest request = WebRequest.Create(validateURL);
                request.Method = "GET";

                WebResponse response = request.GetResponse();

                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                resp = JsonConvert.DeserializeObject<GoogleUserInfo>(responseFromServer);
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (WebException we)
            {
                if (we.Status == WebExceptionStatus.ProtocolError || we.Status == WebExceptionStatus.UnknownError) resp = new GoogleUserInfo() { Id = "-1", CustomerId = "-1" };
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CustomerSecurity.getFBGraph()");
                objErr.LogException();
                resp = new GoogleUserInfo() { Id = "", CustomerId = "-1" };
            }
            return resp;
        }
        /// <summary>
        /// Validates Access token n returns USER info
        /// </summary>
        /// <param name="id">openid</param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static FBGraph getFBGraph(string id, string accessToken)
        {
            FBGraph resp = new FBGraph() ;
            try
            {
                string graphUrl = "https://graph.facebook.com/v2.5/"+ id + "?fields=id,name,email,verified,updated_time,locale,link,last_name,first_name&access_token=" + accessToken;
                WebRequest request = WebRequest.Create(graphUrl);
                request.Method = "GET";

                WebResponse response = request.GetResponse();

                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                resp = JsonConvert.DeserializeObject<FBGraph>(responseFromServer);
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (WebException we)
            {
                if (we.Status == WebExceptionStatus.ProtocolError || we.Status == WebExceptionStatus.UnknownError) resp = new FBGraph() { Id = "-1", CustomerId = "-1" };
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CustomerSecurity.getFBGraph()");
                objErr.LogException();
                resp = new FBGraph() { Id = "-1",CustomerId="-1" };
            }
            return resp;
        }
    }
}
