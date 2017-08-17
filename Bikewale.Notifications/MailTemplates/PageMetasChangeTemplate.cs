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

        public PageMetasChangeTemplate(string makeName, string modelName, String pageName)
        {
            try
            {
                StringBuilder message = new StringBuilder();

                message.Append("<h4>Dear Sir,</h4>");

                if (string.IsNullOrEmpty(modelName))
                {
                    message.Append("<p>Metas changed for " + pageName + " for parameters Make : " + makeName + " and Model :" + modelName + ".</p>");
                }
                else
                {
                    message.Append("<p>Metas changed for " + pageName + " for parameters Make : " + makeName + ".</p>");
                }

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
