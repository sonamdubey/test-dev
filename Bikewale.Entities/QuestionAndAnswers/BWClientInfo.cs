
namespace Bikewale.Entities.QuestionAndAnswers
{
    public class BWClientInfo
    {
        public ushort ApplicationId { get; set; }
        public string ClientIp { get; set; }
        public ushort PlatformId { get; set; }
        public ushort SourceId { get; set; }
        public bool IsInternalUser { get; set; }
    }
}
