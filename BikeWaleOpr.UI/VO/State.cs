using System;
using System.Collections.Generic;
using System.Web;



namespace BikeWaleOpr.VO
{
    /// <summary>
    /// written By : Ashwini Todkar written on 1 Jan 2014
    /// Summary : This Class has all attributes of state entity
    /// </summary>
    public class State
    {
        public string Id { get; set; }
        public string StateName { get; set; }
        public string MaskingName { get; set; }
        public string StdCode { get; set; }
        public bool IsDeleted { get; set; }   

    }
}