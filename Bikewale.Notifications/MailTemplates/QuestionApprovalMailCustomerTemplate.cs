using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 12 June 2018
    /// Description : Mail Template for Email to be sent to the customer once his/her question is approved.
    /// </summary>
    class QuestionApprovalMailCustomerTemplate : ComposeEmailBase
    {
        private string name, questionText, bikeName, dedicatedPageUrl, modelPageUrl;
        public QuestionApprovalMailCustomerTemplate(string name, string questionText, string bikeName, string dedicatedPageUrl, string modelPageUrl)
        {
            this.name = name;
            this.questionText = questionText;
            this.bikeName = bikeName;
            this.dedicatedPageUrl = dedicatedPageUrl;
            this.modelPageUrl = modelPageUrl;
        }
        /// <summary>
        /// Created by  : Sanskar Gupta on 12 June 2018
        /// Description : Mail Body for Email to be sent to the customer once his/her question is approved.
        /// </summary>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>Hi {0},<br><br>", name);
            if (!string.IsNullOrEmpty(bikeName))
            {
                sb.AppendFormat("Thanks for posting your question on <a href=\"{0}\">{1}</a>!<br>", modelPageUrl, bikeName); 
            }
            sb.AppendFormat("We want to let you know that your question is now available for other users to answer. As soon as we get a response for your question, we will let you know.</p>")
                .AppendFormat("<p>Here's what you asked,<br><b>\"{0}\"</b><p>", questionText);

            if (!string.IsNullOrEmpty(dedicatedPageUrl))
            {
                sb.AppendFormat("<p>Check more <a href='{0}'>Questions and Answers on {1}</a> posted by other users on Bikewale.</p>", dedicatedPageUrl, bikeName);
            }

            sb.AppendFormat("<p>Best Regards,<br>Team Bikewale</p>");
            return sb.ToString();
        }
    }
}
