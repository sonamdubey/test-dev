using BikewaleOpr.Entity.ConfigurePageMetas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// created by sajal gupta on 17-08-2017
    /// desc : Added mail template for page metas change
    /// </summary>
    public class PageMetasChangeTemplate : ComposeEmailBase
    {
        private string PageMetasChangeHtml = null;

        public PageMetasChangeTemplate(PageMetasEntity objMetas)
        {
            try
            {
                StringBuilder message = new StringBuilder();

                message.Append("<h4>Dear Sir,</h4>");

                if (objMetas != null)
                {

                    if (!string.IsNullOrEmpty(objMetas.ModelName))
                    {
                        message.Append("<p>Metas changed for " + objMetas.PageName + " for parameters Make : " + objMetas.MakeName + " and Model :" + objMetas.ModelName + ".</p>");
                    }
                    else
                    {
                        message.Append("<p>Metas changed for " + objMetas.PageName + " for parameters Make : " + objMetas.MakeName + ".</p>");
                    }

                    if (!string.IsNullOrEmpty(objMetas.Title))
                        message.Append("<p>New Title : " + objMetas.Title + ".</p>");

                    if (!string.IsNullOrEmpty(objMetas.Description))
                        message.Append("<p>New Description : " + objMetas.Description + ".</p>");

                    if (!string.IsNullOrEmpty(objMetas.Keywords))
                        message.Append("<p>New Keywords : " + objMetas.Keywords + ".</p>");

                    if (!string.IsNullOrEmpty(objMetas.Heading))
                        message.Append("<p>New Heading : " + objMetas.Heading + ".</p>");

                    if (!string.IsNullOrEmpty(objMetas.Summary))
                        message.Append("<p>New Summary : " + objMetas.Summary + ".</p>");

                    PageMetasChangeHtml = message.ToString();
                }

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
