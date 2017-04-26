
namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 20 Mar 2017
    /// Summary : Class have properties related to ad tags
    /// </summary>
    public class AdTags
    {
        #region Ad sizes for desktop
        public string AdId { get; set; }
        public string AdPath { get; set; }

        public string TargetedModel { get; set; }
        public string TargetedModels { get; set; }
        public string TargetedMakes { get; set; }
        public string TargetedCity { get; set; }

        public bool Ad_970x90 { get; set; }
        public bool Ad_970x90BTF { get; set; }
        public bool Ad_970x90Bottom { get; set; }

        public bool Ad_976x400First { get; set; }
        public bool Ad_976x400Second { get; set; }

        public bool Ad_976x204 { get; set; }
        #endregion

        #region Ad sizes for mobile site
        public bool Ad_300x250 { get; set; }
        public bool Ad_300x250BTF { get; set; }

        public bool Ad_Mid_320x50 { get; set; }
        public bool Ad_320x50 { get; set; }
        public bool Ad_Bot_320x50 { get; set; }
        public bool Ad320x150_Top { get; set; }
        public bool Ad320x150_Bottom { get; set; }
        public bool Ad320x100ATF { get; set; }
        public bool Ad320x100BTF { get; set; }

        #endregion

    }
}
