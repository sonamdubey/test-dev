using System;
using System.Text;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created by : Sajal Gupta on 22-12-216
    /// Desc : Send mail to the mentioned bikewale operations user for the bike model units sold data. Class to construct the body for the given mail
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
                message.Append("<p>Model sold unit data was last updated on " + date + "</p>");
                message.AppendFormat("<p><a href=\"{0}/content/bikeunitssold.aspx\">Click here to update the data</a></p>", Bikewale.Utility.BWOprConfiguration.Instance.BwOprHostUrlForJs);

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

