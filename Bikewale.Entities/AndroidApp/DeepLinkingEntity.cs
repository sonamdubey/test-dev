using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Bikewale.Entities.AndroidApp
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 10 March 2016
    /// Description : Used for Android App Deep Link  response.
    /// </summary>
    public class DeepLinkingEntity
    {
        public ScreenIdEnum ScreenID { get; set; }
        public IDictionary<string, string> Params { get; set; }
    }
}
