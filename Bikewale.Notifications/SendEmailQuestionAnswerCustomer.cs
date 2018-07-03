using Bikewale.Notifications.MailTemplates;
using Bikewale.Utility.StringExtention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Notifications
{
    /// <summary>
    /// Created by  : Sanskar Gupta on 12 June 2018
    /// Description : Logic for sending email to a customer when any event is triggered for Questions/Answers
    /// </summary>
    public class SendEmailQuestionAnswerCustomer
    {
        /// <summary>
        /// Created by  : Sanskar Gupta on 12 June 2018
        /// Description : Function for sending email to a customer when his/her question is approved.
        /// </summary>
        public static void SendQuestionApprovalEmail(string userEmail, string name, string questionText, string bikeName, string dedicatedPageUrl, string modelPageUrl)
        {
            ComposeEmailBase objEmail = new QuestionApprovalMailCustomerTemplate(name, questionText, bikeName, dedicatedPageUrl, modelPageUrl);
            objEmail.Send(userEmail, "Here’s a quick update on your question");
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 13 June 2018
        /// Description : Function for sending email to a customer when his/her question is rejected.
        /// </summary>
        public static void SendQuestionRejectionEmail(string userEmail, string name, string questionText, string bikeName, string dedicatedPageUrl)
        {
            ComposeEmailBase objEmail = new QuestionRejectionMailCustomerTemplate(name, questionText, bikeName, dedicatedPageUrl);
            objEmail.Send(userEmail, "Oops! We request you to post your question again");
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 27 June 2018
        /// Description : Function for sending email to a customer when his/her question is answered.
        /// </summary>
        public static void SendQuestionAnsweredEmail(string userEmail, string userName, string questionText, string answerText, string answeredBy, string bikeName, string dedicatedPageUrl, string modelPageUrl)
        {
            ComposeEmailBase objEmail = new QuestionAnsweredMailCustomerTemplate(userName, questionText, answerText, answeredBy, bikeName, dedicatedPageUrl, modelPageUrl);

            int noOfCharactersToBeShownInQuestion = 132;
            questionText = StringHelper.Truncate(questionText, noOfCharactersToBeShownInQuestion);
            if(questionText.Length == noOfCharactersToBeShownInQuestion)
            {
                questionText += "...";
            }
            string subject = string.Format("Congrats! You received a response to your question \"{0}\"", questionText);
            objEmail.Send(userEmail, subject);
        }
    }
}
