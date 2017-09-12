namespace Bikewale.Entities.Finance.CapitalFirst
{
    public class CapitalFirstMakeEntityBase
    {
        public string MakeId { get; set; }
        public string Make { get; set; }
    }

    public class CapitalFirstModelEntityBase
    {
        public string ModelId { get; set; }
        public string ModelNo { get; set; }
    }

    public class CapitalFirstBikeEntity
    {
        public CapitalFirstMakeEntityBase MakeBase { get; set; }
        public CapitalFirstModelEntityBase ModelBase { get; set; }
    }
}
