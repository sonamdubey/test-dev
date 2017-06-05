using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.Compare;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Models.Compare
{
    /// <summary>
    /// Created By :- Subodh Jain 09 May 2017
    /// Summary :- Model For compare bike with body style
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added BL instance for comparison list
    /// </summary>
    /// <returns></returns>
    public class CompareWidget
    {
        private readonly IBikeCompare _objCompare;

        public int topCount { get; set; }
        public uint cityId { get; set; }
        public CompareWidget(IBikeCompare objCompare)
        {
            _objCompare = objCompare;

        }
        public ComparisonWidgetVM GetData()
        {
            ComparisonWidgetVM objComparison = new ComparisonWidgetVM();
            try
            {
                IEnumerable<SimilarCompareBikeEntity> topBikeCompares = null;
                topBikeCompares = _objCompare.GetPopularCompareList(cityId);
                if (topBikeCompares != null && topBikeCompares.Count() > 0)
                {
                    objComparison.topBikeCompares = topBikeCompares.Take(topCount);

                    objComparison.topBikeComparesCruisers = topBikeCompares.Where(x => x.BodyStyle1 == (uint)EnumBikeBodyStyles.Cruiser || x.BodyStyle2 == (uint)EnumBikeBodyStyles.Cruiser);

                    objComparison.topBikeComparesSports = topBikeCompares.Where(x => x.BodyStyle1 == (uint)EnumBikeBodyStyles.Sports || x.BodyStyle2 == (uint)EnumBikeBodyStyles.Sports);

                    objComparison.topBikeComparesScooters = topBikeCompares.Where(x => x.BodyStyle1 == (uint)EnumBikeBodyStyles.Scooter || x.BodyStyle2 == (uint)EnumBikeBodyStyles.Scooter);

                    if (objComparison.topBikeComparesCruisers != null && objComparison.topBikeComparesCruisers.Count() > 0)
                        objComparison.topBikeComparesCruisers = objComparison.topBikeComparesCruisers.Take(topCount);

                    if (objComparison.topBikeComparesSports != null && objComparison.topBikeComparesSports.Count() > 0)
                        objComparison.topBikeComparesSports = objComparison.topBikeComparesSports.Take(topCount);

                    if (objComparison.topBikeComparesScooters != null && objComparison.topBikeComparesScooters.Count() > 0)
                        objComparison.topBikeComparesScooters = objComparison.topBikeComparesScooters.Take(topCount);



                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.CompareBikes.CompareWidget.GetData()");
            }
            return objComparison;
        }
    }
}