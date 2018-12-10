using Carwale.Entity.Enum;
using System;

namespace Carwale.Entity
{
    [Serializable]
    public class CarModelMaskingResponse
    {
        public string MaskingName { get; set; }
        public string MakeName { get; set; }
        public int ModelId { get; set; }
        public int MakeId { get; set; }
        public bool Redirect { get; set; }
        public int RootId { get; set; }
        public string RootName { get; set; }
		public CarStatus Status { get; set; }
    } 
}