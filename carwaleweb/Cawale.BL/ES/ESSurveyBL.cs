using Carwale.Interfaces.ES;
using Carwale.Entity.ES;
using System.Configuration;
using Carwale.Utility;
using System;
using Carwale.Notifications;
using Adtargeting;

namespace Carwale.BL.ES
{
    public class ESSurveyBL : ISurveyBL
    {
        private readonly static int _channelWaitTime = CustomParser.parseIntObject(ConfigurationManager.AppSettings["GrpcChannelWaitTime"] ?? string.Empty);
        private readonly static string[] _e2oCommunicationModels = (ConfigurationManager.AppSettings["E2OCommunicationModels"] ?? "").Split(',');
        private ISurveyRepository _surveyRepo;

        public ESSurveyBL(ISurveyRepository surveyRepo)
        {
            _surveyRepo = surveyRepo;
        }

        public int SaveSurveyData(ESSurveyCustomerResponse Customer, string cwcCookie)        
        {
            int customerId = -1;
            try
            {                
                if (Customer.CampaignId == 2)
                {
                    bool exposed = IsUserExposedToE2o();
                    var client = new UserData.UserDataClient(true);
                    var data = client.IsElectricCarViewed(new CookieRequest { Cookie = cwcCookie }, null, GetForwardTime(_channelWaitTime));
                    bool aware = data.Status;
                    Customer.Comment = aware ? "Aware" : exposed ? "Unaware, but exposed" : "Unaware, not exposed";
                }
                if (Customer.IsFreeTextResponse)
                {
                    customerId = _surveyRepo.SubmitSurveyWithFreeText(Customer);
                }
                else
                    customerId = _surveyRepo.SubmitSurvey(Customer);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ESSurveyBL.SaveSurveyData()\n Exception : " + ex.Message + "CWCCookie: " + cwcCookie);
                objErr.LogException();
            }
            return customerId;
        }

        private bool IsUserExposedToE2o()
        {
            var userModelHistory = CustomerCookie.UserModelHistory.Split('~');
            int length = _e2oCommunicationModels.Length;
            for (int i = 0; i < length; i++)
            {
                if (Array.IndexOf(userModelHistory, _e2oCommunicationModels[i]) >= 0)
                    return true;
            }
            return false;
        }

        private static DateTime GetForwardTime(int incrementMillisecond)
        {
            return DateTime.Now.AddMilliseconds(incrementMillisecond).ToUniversalTime();
        }

        public bool ElectricCars(string cwcCookie)
        {
            try
            {
                var client = new UserData.UserDataClient(true);
                var data = client.IsElectricCarViewed(new CookieRequest { Cookie = cwcCookie }, null, GetForwardTime(_channelWaitTime));
                return data.Status;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ESSurveyBL.ElectricCars()");
                objErr.LogException();
            }
            return false;
        }
    }
}
