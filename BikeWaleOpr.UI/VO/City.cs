using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Author : Ashwini Todkar written on 1st Jan 2014
/// Summary : This class has all attributes of city entity
/// </summary>

namespace BikeWaleOpr.VO
{
    public class City
    {
        public string CityName { get; set; }
        public string CityId { get; set; }
        public string MaskingName { get; set; }
        public string StateId { get; set; }
        public bool IsDeleted { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public string StdCode { get; set; }
        public string DefaultPinCode { get; set; }      
    }
}