using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppNotification.Interfaces;
using AppNotification.Entity;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using Newtonsoft.Json;

namespace AppNotification.BAL
{
    public class AndroidGCMAPI<T, TResponse> : IAPIService<T, TResponse>
        where T : MobileAppNotifications
        where TResponse : APIResponseEntity, new()
    {
       
        public TResponse Request(T t)
        {

            string regIds = String.Join(",", t.GCMList.ToArray());
            string postData = GetGCMData(t);
            string postDataContentType = "application/json";
            string retVal = SendGCMNotification(ConfigurationManager.AppSettings["APIKey"].ToString(), postData, postDataContentType);
            var responseEntity = new TResponse()
            {
                ResponseText = retVal,
            };

            return responseEntity;
        }

        public void UpdateResponse(T t, TResponse t2)
        {
            throw new NotImplementedException();
        }

        public string GetGCMData(T t)
        {
            try
            {
                var gcmDataObj = new GCMFormat();
                gcmDataObj.data = GetGCMBaseData(t);
                gcmDataObj.registration_ids = t.GCMList;
                //Infolog.Info("GetGCMDATA1");
                return JsonConvert.SerializeObject(gcmDataObj);
            }
            catch (Exception ex)
            {
                //Infolog.Info("GetGCMDATA2" + ex.Message);
                return "";
            }
        }

        public MobileAppNotificationBase GetGCMBaseData(T t)
        {
            var gcmBaseDataObj = new MobileAppNotificationBase();
            gcmBaseDataObj.title = t.title;
            gcmBaseDataObj.detailUrl = t.detailUrl;
            gcmBaseDataObj.smallPicUrl = t.smallPicUrl;
            gcmBaseDataObj.alertTypeId = t.alertTypeId;
            gcmBaseDataObj.alertId = t.alertId;
            gcmBaseDataObj.largePicUrl = t.largePicUrl;
            gcmBaseDataObj.isFeatured = t.isFeatured;
            gcmBaseDataObj.publishDate = t.publishDate;

            return gcmBaseDataObj;
        }

        private string SendGCMNotification(string apiKey, string postData, string postDataContentType)
        {
            //If SSL certification is self signed then too the following line of code will Validate the server certificate
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);

            //  MESSAGE CONTENT
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //  CREATE REQUEST
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
            Request.Method = "POST";
            Request.KeepAlive = false;
            Request.ContentType = postDataContentType;
            Request.Headers.Add(String.Format("Authorization: key={0}", apiKey));
            Request.Headers.Add(string.Format("Sender: id={0}", "491935823116"));
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

                return responseLine;
            }
            catch (Exception e)
            {
                responseLine = e.Message.ToString();
            }
            return responseLine;
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }

}
