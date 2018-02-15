
namespace Bikewale.Entities.BikeData.NewLaunched
{
    /// <summary>
    /// Created by  :   Sumit Kate on 13 feb 2017
    /// Description :   New Launched bikes API InputFilter entity
    /// Modified by:- subodh jain 09 march 2017
    /// Summary :- added BodyStyleId 
    /// Modified by : Sanskar Gupta on 31st Jan 2018
    /// Description : Added 'Days' filter.
    /// </summary>
    public class InputFilter
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public uint Make { get; set; }
        public uint YearLaunch { get; set; }
        public uint CityId { get; set; }
        public uint BodyStyle { get; set; }

        public int Days { get; set; }
    }
}
