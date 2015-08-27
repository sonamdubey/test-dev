using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public class CommonApiOpn
    {
        /// <summary>
        /// Written By : Ashwini Todkar on 30 Sept 2014
        /// Summary    : Method to get catagory list in string format
        /// </summary>
        /// <returns></returns>
        public static string GetContentTypesString<T>(List<T> contentList)
        {

            string _contentTypes = string.Empty;
            ushort _contentType = 0;
            if (contentList != null && contentList.Count > 0)
            {
                foreach (var item in contentList)
                {
                    _contentType = Convert.ToUInt16(item);
                    _contentTypes += _contentType.ToString() + ',';
                }

                _contentTypes = _contentTypes.Remove(_contentTypes.LastIndexOf(','));
            }
                
           return _contentTypes;            

        } //End of GetContentTypes
    }
}
