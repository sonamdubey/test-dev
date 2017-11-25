using BikewaleOpr.Entities.BikeData;
using System.Collections.Generic;
using System.Text;

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
            sb.AppendFormat("<table border='1px'  style='border-collapse:collapse;'><tbody><tr><th>Sr. No.</th><th>Bike Name</th><th>Old Url</th><th>New Url</th></tr>");
            
            int i = 1;
            if(models!=null)
            //foreach (var model in models)
            //{
            // if(i>1)
            //    // sb.AppendFormat("<tr><td style='text-align:center'>{0}.</td><td style='padding:5px;'>{1} {2}</td> <td style='padding:5px;'>{3}</td> <td style='padding:5px;'>{4}</td></tr>", i, makeName, model, model.OldUrl, model.NewUrl);
            // else
            //     sb.AppendFormat("<tr><td style='text-align:center'>{0}.</td><td style='padding:5px;'>{1} Bikes</td> <td style='padding:5px;'>{2}</td> <td style='padding:5px;'>{3}</td></tr>", i, makeName, model.OldUrl, model.NewUrl);             
            // i++;
            //}
            sb.AppendFormat("</tbody></table>");
            return sb.ToString();
        }
    }
}
