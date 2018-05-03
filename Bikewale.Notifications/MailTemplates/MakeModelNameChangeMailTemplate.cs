using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Notifications.MailTemplates
{
    class MakeModelNameChangeMailTemplate : ComposeEmailBase
    {
        private string oldName, newName;

        /// <summary>
        /// Initialize the member variables values
        /// </summary>
        public MakeModelNameChangeMailTemplate(string oldName, string newName)
        {
            this.oldName = oldName;
            this.newName = newName;
        }
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>Name of {1} has been changed to {2}.</p>", oldName, newName);
            return sb.ToString();
        }
    }
}
