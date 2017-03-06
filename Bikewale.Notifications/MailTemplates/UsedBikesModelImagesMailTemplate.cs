
using System;
using System.Text;
namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 06 Mar 2017
    /// Desc : Send mail to the mentioned bikewale operations user for the bikes with no used bike model image uploaded
    /// </summary>
    public class UsedBikesModelImagesMailTemplate : ComposeEmailBase
    {
        private string ModelSoldUnitMailHtml = null;
        public UsedBikesModelImagesMailTemplate(string bikeNames)
        {
            try
            {
                StringBuilder message = new StringBuilder();
                message.Append("<h4>Dear User,</h4>");
                message.Append("<p>There are bikes for which Used model image needs to be uploaded.</p> Bikes: ");
                message.Append(bikeNames);
                message.AppendFormat("<p><a href=\"{0}/content/bikeunitssold.aspx\">Click here to update the data</a></p>", Bikewale.Utility.BWOprConfiguration.Instance.BwOprHostUrlForJs);
                ModelSoldUnitMailHtml = message.ToString();
            }
            catch (Exception err)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(err, "Bikewale.Notification.UsedBikesModelImagesMailTemplate.ComposeBody");
            } // catch Exception
        }

        public override string ComposeBody()
        {
            return ModelSoldUnitMailHtml;
        }
    }
}
