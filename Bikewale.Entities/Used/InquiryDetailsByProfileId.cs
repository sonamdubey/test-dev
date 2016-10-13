﻿using System;

namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by : Sajal Gupta on 06-10-2016
    /// Desc  :  For fetching Inquiry Details By ProfileId
    /// </summary>
    [Serializable]
    public class InquiryDetails
    {
        public string ProfileId { get; set; }
        public uint StatusId { get; set; }
        public string CityMaskingName { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public bool IsRedirect { get; set; }
        public string Message { get; set; }
        public string Url { get { return string.Format("/used/bikes-in-{0}/{1}-{2}-{3}/", CityMaskingName, MakeMaskingName, ModelMaskingName, ProfileId); } }
    }
}

