using System.Collections.Generic;

namespace BikewaleOpr.Entity
{
    /// <summary>
    /// Created by Sajal Gupta on 20-11-2017 
    /// Desc : Entity to hold MakeFooter PAge data
    /// </summary>
    public class MakeFooterPageModel
    {
        public IEnumerable<MakeFooterCategory> MakeFooterData { get; set; }
        public string MakeName { get; set; }
        public uint MakeId { get; set; }

    }
}
