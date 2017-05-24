using Bikewale.Notifications.NotificationDAL;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikewaleOpr.Entities.BikeData;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created by : Aditi Srivastava on 23 May 2017
    /// Summary    : Template for email when a model is discontinued
    /// </summary>
    public class MakeMaskingNameChangedMail : ComposeEmailBase
    {
        private IEnumerable<BikeModelMailEntity> models;
        private string makeName;
        /// <summary>
        /// Initialize the member variables values
        /// </summary>
        public MakeMaskingNameChangedMail(string makeName, IEnumerable<BikeModelMailEntity> models)
        {
            this.makeName = makeName;            
            this.models = models;
        }
        /// <summary>
        /// Created by  :   Aditi Srivastava on 23 May 2017
        /// Description :   Prepares the Email Body when model is discontinued
        /// </summary>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var model in models)
            {              
             sb.AppendFormat("<p>The URL of {0} {1} has changed from {2} to {3}.</p>", makeName, model.ModelName, model.OldUrl, model.NewUrl);
            }
            return sb.ToString();
        }
    }
}
