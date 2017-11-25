﻿namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 7th Oct 2015
    /// Summary : NewBike Dealers Base properties 
    /// </summary>
    public class NewBikeDealerEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
        public string PinCode { get; set; }
        public string WorkingHours { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string BikeMake { get; set; }
        public string MakeMaskingName { get; set; }
    }
}
