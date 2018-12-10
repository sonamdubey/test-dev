using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Carwale.UI.Common;
using Ajax;
//using Carwale.Research;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Interfaces.Subscription;
using Carwale.DAL.Subscription;

namespace CarwaleAjax
{
    public class AjaxResearch
    {

        #region Push TD Request to CRM
        /// <summary>
        /// Method to push the customer details to CRM
        /// </summary>
        /// <param name="carName">Name of the car selected</param>
        /// <param name="custName">Customer name</param>
        /// <param name="email">Customer email</param>
        /// <param name="mobile">Customer mobile</param>
        /// <param name="selectedCityId">City selected by the customer</param>
        /// <param name="versionId">Version selected by the customer</param>
        /// <param name="modelId">Model selected by the customer</param>
        /// <param name="makeId">Make selected by the customer</param>
        /// <returns>Bool value</returns>
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool PushCRM(string carName, string custName, string email, string mobile, string selectedCityId, string versionId, string modelId, string makeId, string leadtype = "-1", string cityName = "",string utm ="")
        {
            TestDrive objTD = new TestDrive();
             return objTD.PushCRM(carName, custName, email, mobile, selectedCityId, versionId, modelId, makeId, leadtype, cityName,utm);
        }
        #endregion

        #region Subscribe for alerts
        /// <summary>
        /// Method to subscribe for alerts
        /// </summary>
        /// <param name="emailAddress">Email address for alerts</param>
        /// <param name="subscriptionCategory">Upcoming Cars, News, Road Tests etc</param>
        /// <param name="subscriptionType">Specific to a model or generic</param>
        /// <returns>Boolean</returns>
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public bool Subscribe(string emailAddress, int subscriptionCategory, int subscriptionType)
        {
            SubscriptionRepository subscribeRepo = new SubscriptionRepository();
            return subscribeRepo.Subscribe(emailAddress, subscriptionCategory, subscriptionType);
        }
        #endregion
    }
}