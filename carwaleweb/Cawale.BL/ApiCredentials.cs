using Carwale.Entity.Enum;
using Carwale.Entity.Insurance;
using Carwale.Notifications;
using System;

namespace Carwale.BL
{
    public class ApiCredentials
    {
        protected const string policyBossCar_UserName = "CarWale";
        protected const string policyBossCar_PassWord = "CarWale@123";
        protected const string PolicyBossBike_UserName = "BikeWale";
        protected const string PolicyBossBike_PassWord = "BikeWale@123";
        protected const string PolicyBossDesktop_UserName = "dasktopCarWale";
        protected const string PolicyBossDesktop_PassWord = "dasktopCarWale@123";

        public static Credentials GetCredentials(Platform platformId, Clients clientId)
        {
            var credential = new Credentials();
            if (clientId == Clients.PolicyBoss)
            {
                credential = GetPolicyBossCredentials(platformId);
            }
            return credential;
        }

        private static Credentials GetPolicyBossCredentials(Platform platformId)
        {
            var policyBossCredential = new Credentials();
            try
            {
                if ((platformId == Platform.CarwaleiOS || platformId == Platform.CarwaleAndroid))
                {
                    policyBossCredential.UserName = policyBossCar_UserName;
                    policyBossCredential.Password = policyBossCar_PassWord;
                }
                else if (platformId == Platform.BikewaleDesktop)
                {
                    policyBossCredential.UserName = PolicyBossBike_UserName;
                    policyBossCredential.Password = PolicyBossBike_PassWord;
                }
                else if (platformId == Platform.CarwaleDesktop)
                {
                    policyBossCredential.UserName = PolicyBossDesktop_UserName;
                    policyBossCredential.Password = PolicyBossDesktop_PassWord;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.ApiCredentials.GetPolicyBossCredentials()");
                objErr.LogException();
            }
            return policyBossCredential;
        }
    }
}
