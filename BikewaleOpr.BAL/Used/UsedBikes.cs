using Bikewale.Notifications;
using Bikewale.Notifications.MailTemplates;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.Used;
using System;

namespace BikewaleOpr.BAL.Used
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 06 Mar 2016
    /// BAL for Used bikes functions
    /// </summary>
    public class UsedBikes : IUsedBikes
    {
        public bool SendUnitSoldEmail(SoldUnitData dataObject, string currentUserName)
        {
            bool isShownNotification = false;

            if (dataObject.IsEmailToSend)
            {
                string ccList = Bikewale.Utility.BWOprConfiguration.Instance.NotificationCCUserMailId;
                string[] cc = ccList.Split(',');
                ComposeEmailBase objEmail = new ModelSoldUnitMailTemplate(currentUserName, dataObject.LastUpdateDate);
                objEmail.Send(Bikewale.Utility.BWOprConfiguration.Instance.NotificationToUserMailId, "Please update last month model sold data", "", cc, null);
            }
            if (dataObject.LastUpdateDate.Month != (DateTime.Now.Month - 1))
            {
                isShownNotification = true;
            }
            return isShownNotification;
        }


        public void SendUploadUsedModelImageEmail()
        {
            string[] ccRecievers = Bikewale.Utility.BWOprConfiguration.Instance.NotificationCCUserMailId.Split(',');
            ComposeEmailBase objEmail = new UsedBikesModelImagesMailTemplate();
            objEmail.Send(Bikewale.Utility.BWOprConfiguration.Instance.NotificationToUserMailId, "Please update used bikes - model images", "", ccRecievers, null);
        }
    }
}
