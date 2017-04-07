using Bikewale.Entities.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created by : Aditi Srivastava on 27 Mar 2017
    /// Summary    : To format strings related to editorial pages
    /// </summary>
    public class EditorialsFormatter
    {
        /// <summary>
        /// Created by : Aditi Srivastava on 27 Mar 2017
        /// Summary    : To return content category based on id
        /// </summary>
        public static string GetContentCategory(string contentType)
        {
            string _category = string.Empty;
            EnumCMSContentType _contentType = default(EnumCMSContentType);
           
            if (!string.IsNullOrEmpty(contentType) && Enum.TryParse<EnumCMSContentType>(contentType, true, out _contentType))
                {
                    switch (_contentType)
                    {
                        case EnumCMSContentType.AutoExpo2016:
                        case EnumCMSContentType.News:
                            _category = "NEWS";
                            break;
                        case EnumCMSContentType.Features:
                        case EnumCMSContentType.SpecialFeature:
                            _category = "FEATURES";
                            break;
                        case EnumCMSContentType.ComparisonTests:
                        case EnumCMSContentType.RoadTest:
                            _category = "EXPERT REVIEWS";
                            break;
                        case EnumCMSContentType.TipsAndAdvices:
                            _category = "BIKE CARE";
                            break;
                        default:
                            break;
                    }
                }

             return _category;
        }
    }
}
