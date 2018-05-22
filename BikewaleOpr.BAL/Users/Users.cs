using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entity.Users;
using BikewaleOpr.Interface.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BikewaleOpr.BAL.Users
{
    /// <summary>
    /// Author  : Kartik rathod on 30 march 18
    /// Desc    : BAL class for Users
    /// </summary>
    public class Users : IUsers
    {
        /// <summary>
        /// Author  : Kartik rathod on 30 march 18
        /// Desc    : Google authentication with passed id_token provides authorised emailid
        /// Modifier: allow both domain carwale.com and bikewale.com 
        /// </summary>
        /// <param name="id_token">provided token through google signin api</param>
        /// <returns>valid email id</returns>
        public string GoogleApiAuthentication(string id_token)
        {
            string googleApiTokenInfoUrl = "/oauth2/v3/tokeninfo?id_token={0}", loginId = string.Empty;            

            try
            {
                string requestUri = string.Format(googleApiTokenInfoUrl, id_token);
                string[] allowDomains = new string[] { "carwale.com", "bikewale.com" };

                IDictionary<string, string> userData = null;

                using (BWHttpClient objClient = new BWHttpClient())
                {
                    userData = objClient.GetApiResponseSync<IDictionary<string, string>>(APIHost.GoogleApi, BWConfiguration.Instance.APIRequestTypeJSON, requestUri, userData);
                }

                if (userData != null && userData["email_verified"] == "true" && userData.ContainsKey("hd") && allowDomains.Contains(userData["hd"]))
                {
                    loginId = !string.IsNullOrEmpty(userData["email"]) ? userData["email"].Split('@')[0] : string.Empty;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.Users.GoogleApiAuthentication"));
            }
            return loginId;
        }

        /// <summary>
        /// Author  : Kartik rathod on 30 march 18
        /// Desc    : get opr users details though api/userdetails/get api
        /// </summary>
        /// <param name="loginId">opr loginid</param>
        /// <returns>UserDetailsEntity</returns>
        public UserDetailsEntity GetUserDetails(string loginId)
        {
            string userDetailsApi = "/api/userdetails/get?loginId={0}";

            UserDetailsEntity objUserDetailsEntity = null;

            try
            {
                string requestUri = string.Format(userDetailsApi, loginId);

                using (BWHttpClient objClient = new BWHttpClient())
                {
                    objUserDetailsEntity = objClient.GetApiResponseSync<UserDetailsEntity>(APIHost.CWOPR, BWConfiguration.Instance.APIRequestTypeJSON, requestUri, objUserDetailsEntity);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.Users.GetUserDetails loginId - " + loginId));
            }
            return objUserDetailsEntity;
        }
    }   // class
}   // namespace
