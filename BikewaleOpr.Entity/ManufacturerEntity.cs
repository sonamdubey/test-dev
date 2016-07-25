using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Author  :   Sumit Kate on 01 Feb 2016
    /// Campaign Manufacturer entity
    /// </summary>
    public class ManufacturerEntity
    {
        public int Id { get; set; }
        public string Organization { get; set; }
        public string Name { get; set; }
    }
}