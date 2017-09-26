using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using System.Collections.Generic;

namespace Bikewale.Models.BikeModels
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 26th Sep 2017
    /// Modified by : VM for showing model of similar series for given model
    /// </summary>
    public class SimilarBikeSeriesWidgetVM
    {
        public IEnumerable<SimilarBikeSeriesEntity> SimilarModelSeriesBikes { get; set; }
        public BikeModelEntityBase BikeModel { get; set; }
        public BikeMakeEntityBase BikeMake { get; set; }
        public uint VersionId { get; set; }
        public EnumBikeBodyStyles BodyStyle { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpcoming { get; set; }
        public bool IsDiscontinued { get; set; }
    }
}
