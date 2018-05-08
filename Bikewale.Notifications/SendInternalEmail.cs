using Bikewale.Notifications.MailTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Notifications
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 27 April 2018
    /// Description : Class to hold all the functions used to send email to internal users for monitoring purposes.
    /// </summary>
    public class SendInternalEmail
    {
        static string makeModelNameChangedSubject = "{0} of {1} has been changed to {2}";


        /// <summary>
        /// Created by  : Sanskar Gupta on 27 April 2018
        /// Description : This function will send an email to email address `userEmail` when the field `fieldName` is modified from `oldFieldValue` to `newFieldValue`
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="fieldName"></param>
        /// <param name="oldFieldValue"></param>
        /// <param name="newFieldValue"></param>
        public static void OnFieldChanged(IEnumerable<string> emails, string fieldName, string oldFieldValue, string newFieldValue)
        {
            if(emails == null)
            {
                return;
            }
            ComposeEmailBase objEmail = new MakeModelNameChangeMailTemplate(oldFieldValue, newFieldValue);
            string subject = string.Format(makeModelNameChangedSubject, fieldName, oldFieldValue, newFieldValue);
            foreach (var mail in emails)
            {
                objEmail.Send(mail, subject);
            }
        }
    }

}
