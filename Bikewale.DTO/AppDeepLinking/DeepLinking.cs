using System.Collections.Generic;

namespace Bikewale.DTO.AppDeepLinking
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 11 march 2016
    /// Description : DTO for Deeplinking return Response 
    /// </summary>
    public class DeepLinking
    {
        public ScreenIdEnum ScreenID { get; set; }
        public IDictionary<string, string> Params { get; set; }
    }
}
