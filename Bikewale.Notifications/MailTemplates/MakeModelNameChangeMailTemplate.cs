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
            return string.Format("<p>Name of <b>{0}</b> has been changed to <b>{1}</b>.</p>", oldName, newName);
        }
    }
}
