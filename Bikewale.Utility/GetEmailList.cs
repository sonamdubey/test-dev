using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bikewale.Utility
{
    public class GetEmailList
    {
        public static IEnumerable<string> FetchMailList()
        {
            ICollection<string> mailIDList=new Collection<string>();
            string mails = BWOprConfiguration.Instance.EmailsForBikeChange;
            mailIDList = mails.Split(',');
            return mailIDList;
        }

        /// <summary>
        /// Created by  : Sanskar Gupta on 27 April 2018
        /// Description : Method returning list of emails by splitting the value of a `Web.Config` key.
        /// </summary>
        /// <param name="emails">Value of the key to be splitted around `,` and get the list</param>
        /// <returns></returns>
        public static IEnumerable<string> FetchMailList(string emails)
        {
            return emails != null ? emails.Split(',') : null;
        }
    }
}
