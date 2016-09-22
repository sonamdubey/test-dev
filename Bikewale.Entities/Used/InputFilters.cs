
namespace Bikewale.Entities.Used.Search
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 10 Sept 2016
    /// Class to hold all the raw filter values from the client side for search result page
    /// </summary>
    public class InputFilters
    {
        public uint City { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Budget { get; set; }
        public string Kms { get; set; }
        public string Age { get; set; }
        public string Owner { get; set; }
        public string ST { get; set; }
        public ushort SO { get; set; }
        public int PN { get; set; }
        public int PS { get; set; }
    }
}
