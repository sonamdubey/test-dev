using AppNotification.Entity;
using AppNotification.Interfaces;
using AppNotification.Notifications;
using System;
using System.Collections.Generic;
using System.Configuration;
//using Consumer;

namespace AppNotification.BAL
{
    public class MobileAppAlertService<T> : IRequestManager<T> where T : MobileAppNotifications
    {
        // private static readonly log4net.ILog Infolog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private static readonly log4net.ILog Errorlog = LogManager.GetLogger("ErrorLog");


        private readonly IMobileAppAlertRepository _mobileAppAlertRepo;
        List<string> regKeyList = new List<string>();
        public MobileAppAlertService(IMobileAppAlertRepository mobileAppAlertRepo)
        {
            _mobileAppAlertRepo = mobileAppAlertRepo;
        }

        public void ProcessRequest(T t)
        {
            try
            {
                //Infolog.Info("processe object arrived" + t.alertTypeId + ":" + t.detailUrl + ":" + t.smallPicUrl + ":" + t.title);
                IAPIService< APIResponseEntity> _clientGCMService = new AndroidGCMAPI< APIResponseEntity>();
                //IAPIService<MobileAppNotifications, APIResponseEntity> _clientApnsService = new ApnsPushApi<MobileAppNotifications, APIResponseEntity>();
                ////var SendToApple = ConfigurationManager.AppSettings["SendToApple"].ToString();
                var SendToAndroid = ConfigurationManager.AppSettings["SendToAndroid"].ToString();
                // Infolog.Info("processe object arrived 1 ");
                APIResponseEntity apiResponse;
                // Infolog.Info("processe object arrived 2");
                int alertBatchSize = Int32.Parse(ConfigurationManager.AppSettings["MobileAlertBatchSize"]);
                int totalNumCount = _mobileAppAlertRepo.GetTotalNumberOfSubs(1);
                //Logs.WriteInfoLog(String.Format("user COunt",totalNumCount));
                // Infolog.Info("processe object arrived 3 alertbatchsize" + alertBatchSize +":totalnumcount:"+ totalNumCount);

                int loopCount = totalNumCount / alertBatchSize;

                if (totalNumCount > 0)
                {
                    for (int i = 1; i <= loopCount + 1; i++)
                    {
                        int startIndex = (i - 1) * alertBatchSize + 1;
                        int endIndex = alertBatchSize * i;
                        regKeyList = _mobileAppAlertRepo.GetRegistrationIds(1, startIndex, endIndex);
                        if (SendToAndroid.ToLower() == "true")
                        {
                            apiResponse = _clientGCMService.Request(regKeyList);
                        }
                    }
                    _mobileAppAlertRepo.CompleteNotificationProcess(1);
                }
            }

            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "Bikewale.AppNotification.BAL.ProcessRequest");
                objErr.LogException();
            }
        }

    }
}
