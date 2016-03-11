using System.Collections.Generic;
using Bikewale.DTO.AppDeepLinking;

namespace Bikewale.DTO.AppDeepLinking
{
    public class DeepLinking
    {
        public ScreenIdEnum ScreenID { get; set; }
        public IDictionary<string, string> Params { get; set; }
    }
}
