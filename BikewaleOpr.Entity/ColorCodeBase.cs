namespace BikewaleOpr.Entities
{
    public class ColorCodeBase
    {
        public uint Id { get; set; }
        public uint ModelColorId { get; set; }
        public string HexCode { get; set; }
        public bool IsActive { get; set; }
    }
}