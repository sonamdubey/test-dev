using Carwale.Entity.Enum;

namespace Carwale.Entity.CarData
{
    public class ModelMaskingValidationEntity
    {
        public int ModelId { get; set; }
		public string ModelMaskingName { get; set; }
        public bool IsValid { get; set; }
        public bool IsRedirect { get; set; }
        public string RedirectUrl { get; set; }
		public CarStatus Status { get; set; }
    }
}
