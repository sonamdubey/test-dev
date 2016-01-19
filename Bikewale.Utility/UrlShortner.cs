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
        /// <summary>
        /// Created By : Sadhana Upadhyay on 8 Jan 2016
        /// Summary : To get short url from google api
        /// </summary>
        /// <param name="longUrl"></param>
        /// <returns></returns>
        public UrlShortnerResponse GetShortUrl(string longUrl)
        {
            UrlShortnerResponse objResponse = null;

            string jsonData = "{\"longUrl\":\"" + longUrl + "\"}";

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);
            string contentType = "application/json";
            string responseLine = string.Empty;
            try
            {

                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/urlshortener/v1/url?key=" + BWConfiguration.Instance.GoogleApiKey);
                Request.Method = "POST";
                Request.ContentType = contentType;
                Request.ContentLength = byteArray.Length;
                Stream dataStream = Request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                //  SEND MESSAGE

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
