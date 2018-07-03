using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 13 June 2018
    /// Description : Mail Template for Email to be sent to the customer once his/her question is rejected.
    /// </summary>
    class QuestionRejectionMailCustomerTemplate : ComposeEmailBase
    {
        private string name, questionText, bikeName, dedicatedPageUrl;
        public QuestionRejectionMailCustomerTemplate(string name, string questionText, string bikeName, string dedicatedPageUrl)
        {
            this.name = name;
            this.questionText = questionText;
            this.bikeName = bikeName;
            this.dedicatedPageUrl = dedicatedPageUrl;
        }
        /// <summary>
        /// Created by  : Sanskar Gupta on 13 June 2018
        /// Description : Mail Body for Email to be sent to the customer once his/her question is rejected.
        /// </summary>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>Hi {0},<br>", name)
                .AppendFormat("<p>Here's what you asked,<br><b>\"{0}\"</b><p>", questionText)
                .AppendFormat("<p>Your question doesn’t seem to conform to our guidelines as mentioned below:")
                .AppendFormat("<ul><li>It should be clearly comprehensible. Be as clear as you can in your question so that other users can understand it at first glance</li>")
                .AppendFormat("<li>It should not be abusive, belittling, provocative, or threatening</li>")
                .AppendFormat("<li>It should not contain any kind of promotional content</li>")
                .AppendFormat("<li>It should not be irrelevant to the bike</li>")
                .AppendFormat("<li>It should not use any copyrighted material</li>")
                .AppendFormat("<li>It should not promote any illegal or harmful activity</li></ul><br>")
                .AppendFormat("We regret to inform you that we have removed your question and would request you to post it again as per the above guidelines so that we can help you better. If you have any feedback on the experience, please mail to us at <a href=\"mailto:contact@bikewale.com\">contact@bikewale.com</a>.")
                .AppendFormat("</p>");

            if (!string.IsNullOrEmpty(dedicatedPageUrl))
            {
                sb.AppendFormat("<p>Check more <a href='{0}'>Questions and Answers on {1}</a> posted by other users on Bikewale.</p>", dedicatedPageUrl, bikeName);
            }

            sb.AppendFormat("<p>Best Regards,<br>Team Bikewale</p>");
            return sb.ToString();
        }
    }
}
