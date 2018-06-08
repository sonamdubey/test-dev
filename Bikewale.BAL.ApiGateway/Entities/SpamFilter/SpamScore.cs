
namespace Bikewale.BAL.ApiGateway.Entities.SpamFilter
{
    public class SpamScore
    {
        public float Score { get; set; }
        public ItemScore Name { get; set; }
        public ItemScore Cookie { get; set; }
        public ItemScore Number { get; set; }
        public ItemScore Email { get; set; }
    }

    public class ItemScore
    {
        public float Score { get; set; }
        public string Description { get; set; }
    }
}
