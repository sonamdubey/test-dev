using System;

namespace Carwale.DTOs.Elastic
{
    [Serializable]
    public class FeaturedCar
    {
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public string MakeName { get; set; }
        public int MakeId { get; set; }
        public string MaskingName { get; set; }
        public int RootId { get; set; }
        public string RootName { get; set; }
        public string OutputName { get; set; }
        public bool IsUpcoming { get; set; }
    }
}