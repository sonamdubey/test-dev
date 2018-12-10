namespace Carwale.Entity.CarData
{
    public class VersionMaskingNameValidation
    {
        public int ModelId { get; set; }
        public int VersionId { get; set; }
        public bool IsValid { get; set; }
        public bool IsRedirect { get; set; }
        public string RedirectUrl { get; set; }
    }
}
