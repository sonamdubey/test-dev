using System;

namespace Bikewale.Entities.SEO
{
    [Serializable]
    public class CustomPageMetas
    {
        public uint PageId { get; set; }
        public uint MakeId { get; set; }
        public string MakeMaskingName { get; set; }
        public string MakeName { get; set; }
        public uint ModelId { get; set; }
        public string ModelMaskingName { get; set; }
        public string ModelName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Heading { get; set; }
        public string Summary { get; set; }

    }
}
