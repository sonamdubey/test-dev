using System.Collections.Generic;

namespace Bikewale.Entities.AppDeepLinking
{
    public class DeepLinkingEntity
    {
        public ScreenIdEnum ScreenID { get; set; }
        public IDictionary<string, string> Params { get; set; }
    }
}
