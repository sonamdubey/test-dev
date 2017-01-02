using Bikewale.Notifications;
using Bikewale.Notifications.MailTemplates;
using BikewaleOpr.DALs.Bikedata;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using BikeWaleOpr.Common;
using Microsoft.Practices.Unity;
using System;
using System.Web.UI;

namespace BikeWaleOpr
{
    /// <summary>
    /// Modified By : Sajal Gupta on 23 Dec 2016
    /// </summary>
    public class Default : Page
    {

        protected SoldUnitData dataObj = null;
        protected bool isShownNotification = false;

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Modified by : Sajal gupta on 22-12-2016
        /// Desc : Function modified to show the notifications to the logged in user. Users are fetched from the web.config file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUser.Id == "-1")
            {
                Response.Redirect("/users/login.aspx");
            }
            else if (CurrentUser.Id == Bikewale.Utility.BWOprConfiguration.Instance.NotificationUserId && DateTime.Now.Day > 15) // If customer id matches the user id from the config file then send the notification to the user
            {
                NotificationTrigger();
            }
        }

        /// <summary>
        /// Created by : Sajal Gupta o 22-12-2016
        /// Desc : Function to send a mail to the specific user mentioned in the config file. Also to show the notification in the notification panel for that user only.
        /// </summary>
        protected void NotificationTrigger()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeModels, BikeModelsRepository>();
                IBikeModels _objModelsRepo = container.Resolve<IBikeModels>();

                dataObj = _objModelsRepo.GetLastSoldUnitData();
            }

            if (dataObj.IsEmailToSend)
            {
                string ccList = Bikewale.Utility.BWOprConfiguration.Instance.NotificationCCUserMailId;

                string[] cc = ccList.Split(',');

                ComposeEmailBase objEmail = new ModelSoldUnitMailTemplate(CurrentUser.UserName, dataObj.LastUpdateDate);

                objEmail.Send(Bikewale.Utility.BWOprConfiguration.Instance.NotificationToUserMailId, "Please update last month model sold data", "", cc, null);
            }

            if (dataObj.LastUpdateDate.Month != (DateTime.Now.Month - 1))
            {
                isShownNotification = true;
            }
        }
    }
}
