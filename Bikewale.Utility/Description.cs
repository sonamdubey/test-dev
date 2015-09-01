using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 1 Sept 2015
    /// </summary>
    public static class FormatDescription
    {
        /// <summary>
        /// Function will return the description with truncated content.
        /// </summary>
        /// <param name="_desc"></param>
        /// <returns></returns>
        public static string TruncateDescription(string _desc)
        {
            _desc = Regex.Replace(_desc, @"<[^>]+>", string.Empty);

            if (_desc.Length < 170)
                return _desc;
            else
            {
                _desc = _desc.Substring(0, 165);
                _desc = _desc.Substring(0, _desc.LastIndexOf(" "));
                return _desc + " [...]";
            }
        }
    }
}
