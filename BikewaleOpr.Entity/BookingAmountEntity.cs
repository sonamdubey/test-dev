using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Created By : Ashwini Todkar 17 Dec 2014
    /// </summary>
    public class BookingAmountEntity
    {
        public BookingAmountEntityBase BookingAmountBase { get; set; }
        public BikeMakeEntityBase BikeMake { get; set; }
        public BikeModelEntityBase BikeModel { get; set; }
        public BikeVersionEntityBase BikeVersion { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public NewBikeDealers NewBikeDealers { get; set; }
        public uint DealerId { get; set; }
    }
}
