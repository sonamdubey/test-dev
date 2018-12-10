using Carwale.Notifications.Logs;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace Carwale.Notifications.Emails
{
    public class CheetahMailClient
    {
        private static string _cheetahMailUrl = ConfigurationManager.AppSettings["CheetahMailUrl"];
        private static string _authUrl = ConfigurationManager.AppSettings["CheetahMailAuthUrl"];
        private Cookie _authCookie;

        public bool SendMail(string mailData)
        {
            if (_authCookie == null || _authCookie.Expired)
            {
                _authCookie = GetAuthCookie();
            }

            byte[] buffer = Encoding.ASCII.GetBytes(mailData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_cheetahMailUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = buffer.Length;
            request.Headers["Cookie"] = _authCookie.ToString();

            Stream reqStream = request.GetRequestStream();
            reqStream.Write(buffer, 0, buffer.Length);
            reqStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream);
            string responseMessage = streamReader.ReadToEnd();

            if (responseMessage.Contains("OK"))
            {
                return true;
            }
            else
            {
                Logger.LogError("Cheetah mail response: " + responseMessage + " for mail: " + mailData);
                return false;
            }
        }

        private Cookie GetAuthCookie()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_authUrl);
            request.CookieContainer = new CookieContainer();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response != null && response.Cookies.Count > 0)
            {
                return response.Cookies[response.Cookies.Count - 1];
            }
            return null;
        }
    }
}
