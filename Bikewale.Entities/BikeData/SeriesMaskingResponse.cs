using System;
namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 20 Nov 2017
    /// Summary : Class to hold series and model masking name properties
    /// Modified by :   Sumit Kate on 11 Dec 2017
    /// Description :   Added Make Masking name
    /// Modified by :   Sumit Kate on 12 Dec 2017
    /// Description :   Added MakeId, series id, modelid and bodystyle id
    /// </summary>
    [Serializable]
    public class SeriesMaskingResponse
    {
        /// <summary>
        /// Value would be series or model masking name. This value should be used for lookup
        /// </summary>
        public string MaskingName { get; set; }

        /// <summary>
        /// Value would be new masking name. Used for redirection in case of masking name is actually old masking and needs redirection
        /// </summary>
        public string NewMaskingName { get; set; }

        /// <summary>
        /// Flag to identify whether series page is created for given masking name or not
        /// </summary>
        public bool IsSeriesPageCreated { get; set; }

        /// <summary>
        /// Value can be 200 or 301. Based on this value redirection of the url can be done
        /// </summary>
        public ushort StatusCode { get; set; }

        /// <summary>
        /// Value would be name of series or model depending on the masking name redirection
        /// </summary>
        public string Name { get; set; }

        public string MakeMaskingName { get; set; }

        public string MakeName { get; set; }
        public uint MakeId { get; set; }
        public uint ModelId { get; set; }
        public uint SeriesId { get; set; }
        public Bikewale.Entities.GenericBikes.EnumBikeBodyStyles BodyStyle { get; set; }
    }
}
