
namespace Carwale.Entity.Classified.CarValuation
{
    public class ValuationUrlParameters 
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int AskingPrice { get; set; }
        public int VersionId { get; set; }
        public short Year { get; set; }
        public int CityId { get; set; }
        public int Owners { get; set; }
        public int Kilometers { get; set; }
        public string ProfileId { get; set; }
        public bool IsSellingIndex { get; set; }
    }
}
