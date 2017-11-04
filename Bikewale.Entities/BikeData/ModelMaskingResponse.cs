namespace Bikewale.Entities.BikeData
{

    public class ModelMaskingResponse
    {
        public uint ModelId { get; set; }
        public string MaskingName { get; set; }
        //public bool Redirect { get; set; }
        public ushort StatusCode { get; set; }
    }
}
