using System.Text;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created by : Aditi Srivastava on 23 May 2017
    /// Summary    : Template for email when a model is discontinued
    /// </summary>
    public class ModelMaskingNameChangedMail : ComposeEmailBase
    {
         private string 
            makeName,
            modelName,
            oldUrl,
            newUrl;
        /// <summary>
        /// Initialize the member variables values
        /// </summary>
         public ModelMaskingNameChangedMail(string makeName, string modelName, string oldUrl, string newUrl)
        {
            this.makeName = makeName;
            this.modelName = modelName;
            this.oldUrl = oldUrl;
            this.newUrl = newUrl;
        }
        /// <summary>
        /// Created by  :   Aditi Srivastava on 23 May 2017
        /// Description :   Prepares the Email Body when masking name is changed
        /// </summary>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>The URL of {0} {1} has changed from {2} to {3}</p>", makeName,modelName,oldUrl,newUrl);
            return sb.ToString();
        }
    }
}
