using Bikewale.Entities.UrlShortner;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public class UrlShortner
    {
        public static UrlShortnerResponse GetShortUrl(string longUrl)
        {
            string shortUrl = string.Empty;

            UrlShortnerResponse objResponse = null;

            string jsonData = "{\"longUrl\":\"" + longUrl + "\"}";

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);
            string contentType = "application/json";
            
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/urlshortener/v1/url?key=" + BWConfiguration.Instance.GoogleApiKey);
            Request.Method = "POST";
            Request.ContentType = contentType;
            Request.ContentLength = byteArray.Length;
            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            string responseLine = string.Empty;

            //  SEND MESSAGE
            try
            {
                WebResponse Response = Request.GetResponse();
                StreamReader Reader = new StreamReader(Response.GetResponseStream());
                responseLine = Reader.ReadToEnd();
                Reader.Close();

                objResponse = JsonConvert.DeserializeObject<UrlShortnerResponse>(responseLine);
            }
            catch (Exception e)
            {
                responseLine = e.Message.ToString();
            }

            return objResponse;
        }
    }
}
