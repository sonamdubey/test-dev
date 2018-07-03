using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 27 June 2018
    /// Description : Mail Template to be sent to the user once his/her question is answered.
    /// </summary>
    public class QuestionAnsweredMailCustomerTemplate : ComposeEmailBase
    {
        private string userName, questionText, answerText, answeredBy, bikeName, dedicatedPageUrl, modelPageUrl;

        public QuestionAnsweredMailCustomerTemplate(string userName, string questionText, string answerText, string answeredBy, string bikeName, string dedicatedPageUrl, string modelPageUrl)
        {
            this.userName = userName;
            this.questionText = questionText;
            this.answerText = answerText;
            this.answeredBy = answeredBy;
            this.bikeName = bikeName;
            this.dedicatedPageUrl = dedicatedPageUrl;
            this.modelPageUrl = modelPageUrl;
        }

        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>Hi {0},<p>", !string.IsNullOrEmpty(userName) ? userName : "there");
            if (!string.IsNullOrEmpty(bikeName)) {
                sb.AppendFormat("<p>We are glad to inform you that one of our users answered your question related to <a href=\"{0}\">{1}</a></p>", modelPageUrl, bikeName);
            }
            sb.AppendFormat("<p>Here's what you asked,<br><b>\"{0}\"</b></p>", questionText)
                .AppendFormat("<p>{0} responded :<br><b>\"{1}\"</b></p>", answeredBy, answerText);

            if (!string.IsNullOrEmpty(dedicatedPageUrl))
            {
                sb.AppendFormat("<p>Read more <a href='{0}'>Questions and Answers on {1}</a> posted by other users on Bikewale.</p>", dedicatedPageUrl, bikeName);
            }

            sb.AppendFormat("<p>Best Regards,<br>Team Bikewale</p>");
            return sb.ToString();
        }
    }
}
