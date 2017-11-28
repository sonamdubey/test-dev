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
    }
}
