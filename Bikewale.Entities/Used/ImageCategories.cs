﻿
using System.ComponentModel;
namespace Bikewale.Entities.Used
{
    public enum ImageCategories
    {
        BIKEWALESELLER = 1
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
}
