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
        /// Created by : Sanskar Gupta on 27 April 2018
        /// Description : 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<string> FetchMailList(string key)
        {
            ICollection<string> mailIDList = new Collection<string>();
            mailIDList = key.Split(',');
            return mailIDList;
        }
    }
}
