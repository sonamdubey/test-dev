using Bikewale.DTO.AutoBiz;

namespace BikeWale.DTO.AutoBiz
{
    public class DealerPriceEntityDTO
    {
        public PQ_PriceDTO Price { get; set; }
        public VersionEntityBaseDTO Version { get; set; }
        public CityEntityBaseDTO City { get; set; }
    }
}
