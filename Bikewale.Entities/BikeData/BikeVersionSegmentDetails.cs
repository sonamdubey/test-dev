
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// created by sajal gupta for version segment details
    /// </summary>
    public class BikeVersionSegmentDetails
    {
        private string _Segment;
        public string Segment { get { return string.IsNullOrEmpty(_Segment) ? "NA" : _Segment; } }
        private string _VersionName;
        public string VersionName { get { return string.IsNullOrEmpty(_VersionName) ? "NA" : _VersionName; } }
        public uint VersionId { get; set; }
        public string BodyStyle { get; set; }

        public BikeVersionSegmentDetails(string seg, string vn)
        {
            _Segment = seg;
            _VersionName = vn;
        }
    }
}
