using System;

namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by : Sajal Gupta on 06-10-2016
    /// Desc  :  For fetching Inquiry Details By ProfileId
    /// </summary>
    [Serializable]
    public class InquiryDetails
    {
        public uint StatusId { get; set; }
        public string CityMaskingName { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
    }
}

