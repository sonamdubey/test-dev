using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Notifications.MailTemplates
{
    public class PageMetasChangeTemplate : ComposeEmailBase
    {
        private string PageMetasChangeHtml = null;

        public PageMetasChangeTemplate(string makeName, string modelName, String pageName, string title, string description, string keywords, string heading, string summary)
        {
            try
            {
                StringBuilder message = new StringBuilder();

                message.Append("<h4>Dear Sir,</h4>");

                if (!string.IsNullOrEmpty(modelName))
                {
                    message.Append("<p>Metas changed for " + pageName + " for parameters Make : " + makeName + " and Model :" + modelName + ".</p>");
                }
                else
                {
                    message.Append("<p>Metas changed for " + pageName + " for parameters Make : " + makeName + ".</p>");
                }

                if (!string.IsNullOrEmpty(title))
                    message.Append("<p>New Title : " + title + ".</p>");

                if (!string.IsNullOrEmpty(description))
                    message.Append("<p>New Description : " + description + ".</p>");

                if (!string.IsNullOrEmpty(keywords))
                    message.Append("<p>New Keywords : " + keywords + ".</p>");

                if (!string.IsNullOrEmpty(heading))
                    message.Append("<p>New Heading : " + heading + ".</p>");

                if (!string.IsNullOrEmpty(summary))
                    message.Append("<p>New Summary : " + summary + ".</p>");

                PageMetasChangeHtml = message.ToString();

            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Notification.PageMetasChangeTemplate.PageMetasChangeTemplate");
            }

        }

        public override string ComposeBody()
        {
            return PageMetasChangeHtml;
        }
    }
}
