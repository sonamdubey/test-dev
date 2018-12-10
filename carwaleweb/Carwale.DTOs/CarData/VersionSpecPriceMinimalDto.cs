namespace Carwale.DTOs.CarData
{
    //Used in Versions list on Desktop PQ page
    public class VersionSpecPriceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FormattedPrice { get; set; }
        public int Price { get; set; }
        public string FuelType { get; set; }
        public string Transmission { get; set; }
    }
}
