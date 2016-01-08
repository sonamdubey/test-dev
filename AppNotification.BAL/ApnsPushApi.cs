using AppNotification.Entity;
using AppNotification.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotification.BAL
{
    class ApnsPushApi<T, TResponse> : IAPIService<T, TResponse>
        where T : MobileAppNotifications
        where TResponse : APIResponseEntity, new()
    {

        public TResponse Request(T t)
        {
            #region puchsharp


            try
            {
                //string path = ConfigurationManager.AppSettings["ApnsCertiPath"].ToString();
                //bool isDev = Convert.ToBoolean(ConfigurationManager.AppSettings["IsDevelopment"]);
                //var appleCert = File.ReadAllBytes(path);
                //var push = new PushBroker();
                //push.RegisterAppleService(new ApplePushChannelSettings(!isDev, appleCert, "carwale"));
                //foreach (string item in t.ApnsList)
                //{

                //    push.QueueNotification(new AppleNotification()
                //                                .ForDeviceToken(item)//the recipient device id
                //                                .WithAlert(t.title)//the message
                //                                .WithSound("sound.caf")
                //                                .WithCustomItem("pushTitle", t.title)
                //                                .WithCustomItem("detailUrl", t.detailUrl)
                //                                .WithCustomItem("smallPicUrl", t.smallPicUrl)
                //                                .WithCustomItem("alertTypeId", t.alertTypeId)
                //                                .WithCustomItem("alertId", t.alertId)
                //                                .WithCustomItem("largePicUrl", t.largePicUrl)
                //                                .WithCustomItem("isFeatured", t.isFeatured)
                //                                .WithCustomItem("publishDate", t.publishDate)
                //                                );
                //}
            }
            catch (Exception ex)
            {
                //var objErr = new ExceptionHandler(ex, "Carwale.Service.ApplePushNotification");
                //objErr.LogException();
            }



            #endregion

           
            var responseEntity = new TResponse()
            {
                ResponseText = "",
            };

            return responseEntity;
        }

        public void UpdateResponse(T t, TResponse t2)
        {
            throw new NotImplementedException();
        }
    }
}
