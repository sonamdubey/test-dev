using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
