namespace Bikewale.Comparison.Entities
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Aug 2017
    /// Description :   Target version Entity
    /// </summary>
    public class TargetVersion
    {
        public uint TargetMakeId { get; set; }
        public uint TargetModelId { get; set; }
        public uint TargetVersionId { get; set; }
        public string TargetMakeName { get; set; }
        public string TargetModelName { get; set; }
        public string TargetVersionName { get; set; }
    }
}
