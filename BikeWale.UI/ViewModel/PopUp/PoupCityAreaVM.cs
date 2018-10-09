namespace Bikewale.Models
{
    /// <summary>
    /// Modified by : Sanskar Gupta on 16 May 2018
    /// Description : Added property `MakeId`
    /// Modified By : Prabhu Puredla on 10 oct 2018
    /// Description : Added ExitUrl property for amp pages for amp pages when user clicks back 
    /// </summary>
    public class PoupCityAreaVM : ModelBase
    {
        public uint ModelId { get; set; }

        public string MakeName { get; set; }

        public string ModelName { get; set; }

        public bool IsPersistent { get; set; }

        public uint PQSourceId { get; set; }

        public uint PageCategoryId { get; set; }

        public uint PreSelectedCity { get; set; }

        public bool IsReload { get; set; }

        public string Url { get; set; }

        public uint MakeId { get; set;}

        public string ExitUrl { set; get; }


    }
}
