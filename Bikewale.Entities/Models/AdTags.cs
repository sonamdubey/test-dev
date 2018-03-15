﻿
namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 20 Mar 2017
    /// Summary : Class have properties related to ad tags
    /// Modified By : Rajan Chauhan on 18 Jan 2018
    /// Description : Added Ad_Model_Bottom_970x90 and Ad_Model_Comparo_976x400 fields
    /// Modified By : Pratibha Verma on 30th Jan 2018
    /// Description : Added Ad_320x50BTF field
    /// Modified by : Snehal Dange on 16th Jan 2017
    /// Description: Added ad tags for make page
    /// Modified By : Deepak Israni on 27 Feb 2018
    /// Description: Added Ad_300x250_Bottom, Ad_300x250_Top, Ad_300x250_Middle and Ad_320x100_Top. Rearranged mobile properties wrt size.
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
        public string TargetedSeries { get; set; }
        public string TargetedTags { get; set; }

        public bool Ad_970x90 { get; set; }
        public bool Ad_970x90Top { get; set; }
        public bool Ad_970x90BTF { get; set; }
        public bool Ad_970x90Bottom { get; set; }

        public bool Ad_976x400First { get; set; }
        public bool Ad_976x400Second { get; set; }

        public bool Ad_Model_ATF_970x90 { get; set; }
        public bool Ad_Model_ATF_300x250 { get; set; }
        public bool Ad_Model_BTF_970x90 { get; set; }
        public bool Ad_Model_BTF_300x250 { get; set; }
        public bool Ad_976x204 { get; set; }
        public bool Ad_976x400_Middle { get; set; }
        public bool Ad_Model_Comparo_976x400 { get; set; }
        public bool Ad_Model_Bottom_970x90 { get; set; }

        public bool Ad_292x399 { get; set; }
        public bool Ad_292x359 { get; set; }

        public bool Ad_292x360 { get; set; }
        public bool Ad_Top_300x250 { get; set; }

        public bool ShowInnovationBannerDesktop { get; set; }
        #endregion

        #region Ad sizes for mobile site
        public bool Ad_200x211 { get; set; }
        public bool Ad_200x216 { get; set; }
        public bool Ad_200x253 { get; set; }    

        public bool Ad_300x100 { get; set; }

        public bool Ad_300x250 { get; set; }
        public bool Ad_300x250BTF { get; set; }
        public bool Ad300x250_Bottom { get; set; }
        public bool Ad_300x250_Bottom { get; set; }
        public bool Ad_300x250_Top { get; set; }
        public bool Ad_300x250_Middle { get; set; }
        

        public bool Ad_320x50BTF { get; set; }
        public bool Ad_Mid_320x50 { get; set; }
        public bool Ad_320x50 { get; set; }
        public bool Ad_320x50Top { get; set; }
        public bool Ad_Bot_320x50 { get; set; }
        public bool Ad_Body_Top_320x50 { get; set; }

        public bool Ad320x100ATF { get; set; }
        public bool Ad320x100BTF { get; set; }
        public bool Ad_320x100_Top { get; set; }
        
        public bool Ad320x150_Top { get; set; }
        public bool Ad320x150_Bottom { get; set; }
        
        public bool Ad_320x400_Middle { get; set; }
        

        public bool ShowInnovationBannerMobile { get; set; }
        #endregion
        public string InnovationBannerGALabel { get; set; }
    }
}
