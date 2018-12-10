using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Enum
{
    public enum ImageCategories
    {
        TRADINGCARS = 1,
        EDITCMS = 2,
        SELLCARINQUIRY = 3,
        MANAGEWEBSITE40 = 40,
        MANAGEWEBSITE41 = 41,
        MANAGEWEBSITE42 = 42,
        MANAGEWEBSITE43 = 43,
        MANAGEWEBSITE44 = 44,
        MANAGEWEBSITE45 = 45,
        CWCOMMUNITY = 5,
        FORUMSREALIMAGE = 6,
        USEDSELLCARS = 7
    }

    public enum ImageKeys
    {
        /// <summary>
        /// total 11 of which last two are not used if resizing is not been done on rabbitmq servers they are only used in case of replication
        /// </summary>
        [Description("id")]
        ID,
        [Description("category")]
        CATEGORY,
        [Description("location")]
        LOCATION,
        [Description("customsizewidth")]
        CUSTOMSIZEWIDTH,
        [Description("customsizeheight")]
        CUSTOMSIZEHEIGHT,
        [Description("iswatermark")]
        ISWATERMARK,
        [Description("iscrop")]
        ISCROP,
        [Description("ismain")]
        ISMAIN,
        [Description("saveoriginal")]
        SAVEORIGINAL,
        [Description("imagetargetpath")]
        IMAGETARGETPATH,
        [Description("onlyreplicate")]
        ONLYREPLICATE,
        [Description("ismaster")]
        ISMASTER,
        [Description("aspectratio")]
        ASPECTRATIO

    }

    public enum ImageTypes
    {
        Interior =1,
        Exterior =2
    }
}
