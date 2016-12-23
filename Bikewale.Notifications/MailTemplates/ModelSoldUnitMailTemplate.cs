using System;
using System.Text;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created by : Sajal Gupta on 22-12-216
    /// Desc : Send mail to nandu and abhishek for last month model sold unit not updated.
    /// </summary>
    public class ModelSoldUnitMailTemplate : ComposeEmailBase
    {
        private string ModelSoldUnitMailHtml = null;

        public ModelSoldUnitMailTemplate(string customerName, DateTime date)
        {
            try
            {
                StringBuilder message = new StringBuilder();

                message.Append("<h4>Dear " + customerName + ",</h4>");

                message.Append("<p>Please update Last month model sold unit data.<br> It was last updated on " + date + "</p>");

                ModelSoldUnitMailHtml = message.ToString();
            }
            catch (Exception err)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(err, "Bikewale.Notification.ModelSoldUnitMail.ComposeBody");
                objErr.SendMail();
            } // catch Exception
        }

        public override string ComposeBody()
        {
            return ModelSoldUnitMailHtml;
        }
    }
}

