using System.Collections.Generic;

namespace Bikewale.Entities.AppDeepLinking
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 11 march 2016
    /// Description : Entity for Deeplinking return Response 
    /// </summary>
    public class DeepLinkingEntity
    {
        public ScreenIdEnum ScreenID { get; set; }
        public IDictionary<string, string> Params { get; set; }
    }
}
