using System;
using System.Collections.Generic;
using System.Text;

namespace Bikewale.Utility
{
    public class CommonApiOpn
    {
        /// <summary>
        /// Written By : Ashwini Todkar on 30 Sept 2014
        /// Summary    : Method to get catagory list in string format
        /// </summary>
        /// <returns></returns>
        public static string GetContentTypesString<T>(IList<T> contentList)
        {
            StringBuilder _contentTypes = new StringBuilder();
            if (contentList != null && contentList.Count > 0)
            {
                foreach (var item in contentList)
                {
                    _contentTypes.Append(Convert.ToUInt16(item)).Append(',');
                }
            }

            return _contentTypes.ToString().TrimEnd(',');

        } //End of GetContentTypes
    }
}
