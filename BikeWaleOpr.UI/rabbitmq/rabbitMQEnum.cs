using System;
using System.ComponentModel;
using System.Web;

namespace BikeWaleOpr.RabbitMQ
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 31th Dec 2013
    /// Summary : ImageKey for rabbitMQ variable
    /// </summary>
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
        [Description("isspecialimage")]
        ISSPECIALIMAGE,
        [Description("ismaster")]
        ISMASTER,
        [Description("aspectratio")]
        ASPECTRATIO
    }

    /// <summary>
    /// Created By : Sadhana Upadhyay on 31th Dec 2013
    /// Summary : to set category and category id
    /// </summary>
    public enum ImageCategories
    {
        BIKEWALESELLER = 1,
        BIKEVERSION = 2,
        EXPECTEDLAUNCH = 3,
        FEATUREDLISTING = 4,
        EDITCMS = 5,
        BIKECOMPARISIONLIST = 6,
        BIKESERIES = 7
    }
}   //End of namespace
