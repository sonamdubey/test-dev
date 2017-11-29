using System;
using System.Text;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created by : Aditi Srivastava on 23 May 2017
    /// Summary    : Template for email when a model is discontinued
    /// </summary>
    public class ModelDiscontinuedMail : ComposeEmailBase
    {
        private string 
            makeName,
            modelName,
            date;
        /// <summary>
        /// Initialize the member variables values
        /// </summary>
        public ModelDiscontinuedMail(string makeName, string modelName,DateTime date)
        {
            this.makeName = makeName;
            this.modelName = modelName;
            this.date = date.ToString("dd MMM yyyy");
        }
        /// <summary>
        /// Created by  :   Aditi Srivastava on 23 May 2017
        /// Description :   Prepares the Email Body when model is discontinued
        /// </summary>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>{0} {1} has got discontinued on {2} on Bikewale.</p>", makeName,modelName,date);
            return sb.ToString();
        }
    }
}
