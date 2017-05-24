using Bikewale.Notifications.MailTemplates;
using BikewaleOpr.Entities.BikeData;
using System;
using System.Collections.Generic;

namespace Bikewale.Notifications
{
    /// <summary>
    /// Created by : Aditi Srivastava on 23 May 2017
    /// Summary    : Mail notification functions for model discontinuation and masking name change
    /// </summary>
    public class SendEmailOnModelChange
    {
        /// <summary>
        /// Send email on model discontinuation
        /// </summary>
        public static void SendModelDiscontinuedEmail(string userEmail, string makeName, string modelName,DateTime date)
        {
            ComposeEmailBase objEmail = new ModelDiscontinuedMail(makeName, modelName, date);
            objEmail.Send(userEmail, string.Format("{0} {1} has got discontinued", makeName, modelName));
        }

        /// <summary>
        /// Send email on model masking name change
        /// </summary>
        public static void SendModelMaskingNameChangeMail(string userEmail, string makeName, string modelName,string oldUrl,string newUrl)
        {
            ComposeEmailBase objEmail = new ModelMaskingNameChangedMail(makeName, modelName, oldUrl, newUrl);
            objEmail.Send(userEmail, string.Format("URL of {0} {1} has changed", makeName, modelName));
        }

        /// <summary>
        /// Send email on make masking name change
        /// </summary>
        public static void SendMakeMaskingNameChangeMail(string userEmail, string makeName, string makeMasking,IEnumerable<BikeModelMailEntity> models )
        {
            ComposeEmailBase objEmail = new MakeMaskingNameChangedMail(makeName, makeMasking, models);
            objEmail.Send(userEmail, string.Format("URL of {0} bikes have changed", makeName));
        }
    }
}
