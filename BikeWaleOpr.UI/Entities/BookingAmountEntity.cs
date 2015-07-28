using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeWaleOpr.Entities
{
    /// <summary>
    /// Created By : Suresh Prajapati on 31st Dec 2014.
    /// </summary>
    [Serializable]
    public class BookingAmountEntity
    {
        public BookingAmountEntityBase objBookingAmountEntityBase { get; set; }
        public NewBikeDealers objDealer { get; set; }
        public MakeEntityBase objMake { get; set; }
        public ModelEntityBase objModel { get; set; }
        public VersionEntityBase objVersion { get; set; }
    }
}
