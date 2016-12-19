using AppNotification.Entity;
using AppNotification.Interfaces;
using AppNotification.Notifications;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
//using Consumer;

namespace AppNotification.BAL
{
    public class MobileAppAlertService : IRequestManager
    {
        // private static readonly log4net.ILog Infolog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private static readonly log4net.ILog Errorlog = LogManager.GetLogger("ErrorLog");


        private readonly IMobileAppAlertRepository _mobileAppAlertRepo;
        List<string> regKeyList = new List<string>();
        public MobileAppAlertService(IMobileAppAlertRepository mobileAppAlertRepo)
        {
            _mobileAppAlertRepo = mobileAppAlertRepo;
        }

        public void ProcessRequest()
        {
            try
            {
                //Infolog.Info("processe object arrived" + t.alertTypeId + ":" + t.detailUrl + ":" + t.smallPicUrl + ":" + t.title);
                IAPIService<SubscriptionResponse> _clientGCMService = new AndroidGCMAPI<SubscriptionResponse>();
                //Infolog.Info("processe object arrived 1 ");
                SubscriptionResponse apiResponse;
                //APIResponseEntity apiResponse;
                int alertBatchSize = Int32.Parse(ConfigurationManager.AppSettings["MobileAlertBatchSize"]);
                int totalNumCount = _mobileAppAlertRepo.GetTotalNumberOfSubs(1);
                // Infolog.Info("processe object arrived 3 alertbatchsize" + alertBatchSize + ":totalnumcount:" + totalNumCount);

                int loopCount = totalNumCount / alertBatchSize;

                if (totalNumCount > 0)
                {
                    for (int i = 1; i <= loopCount + 1; i++)
                    {
                        int startIndex = (i - 1) * alertBatchSize + 1;
                        int endIndex = alertBatchSize * i;
                        regKeyList = _mobileAppAlertRepo.GetRegistrationIds(1, startIndex, endIndex);

                        apiResponse = _clientGCMService.Request(regKeyList);
                        StringBuilder sb = new StringBuilder();
                        for (int value = 0; value < apiResponse.Results.Count(); value++)
                        {
                            sb.Append("," + regKeyList[value] + ":" + (string.IsNullOrEmpty(apiResponse.Results[value].Error) ? "SUCCESS" : apiResponse.Results[value].Error));
                        }
                        _mobileAppAlertRepo.CompleteNotificationProcess(1, Convert.ToString(sb).Substring(1));

                    }

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
