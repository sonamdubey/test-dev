using Bikewale.BAL.GrpcFiles.Specs_Features;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.NewLaunched;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 25-Mar-2017
    /// Summary: Widget for new lauched bikes on pages . For ex. Homepage
    /// </summary>
    public class NewLaunchedWidgetModel
    {
        public IEnumerable<NewLaunchedBikeEntityBase> Bikes { get; set; }
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private ushort _recordCount;
        public uint BodyStyleId { get; set; }
        public uint MakeId { get; set; }
        private readonly uint _cityId;

        public NewLaunchedWidgetModel(ushort recordCount,INewBikeLaunchesBL newLaunches)
        {
            _newLaunches = newLaunches;
            _recordCount = recordCount;
        }

        public NewLaunchedWidgetModel(ushort recordCount, uint cityId, INewBikeLaunchesBL newLaunches)
        {
            _newLaunches = newLaunches;
            _recordCount = recordCount;
            _cityId = cityId;
        }

        public NewLaunchedWidgetModel(uint makeId, uint cityId, ushort recordCount, INewBikeLaunchesBL newLaunches)
        {
            _newLaunches = newLaunches;
            _recordCount = recordCount;
            MakeId = makeId;
            _cityId = cityId;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
        /// Modified by : Pratibha Verma on 28 Mar 2018
        /// Description : Added grpc method call to get MinSpecs data
        /// </returns>
        public NewLaunchedWidgetVM GetData()
        {
            NewLaunchedWidgetVM objData = new NewLaunchedWidgetVM();
            var objFilters = new InputFilter()
            {
                PageNo = 1,
                BodyStyle = BodyStyleId,
                PageSize = _recordCount,
                Make = MakeId,
                CityId = _cityId
            };
            var bikeBase = _newLaunches.GetBikes(objFilters);
            objData.Bikes = bikeBase.Bikes;
            if (objData.Bikes != null && objData.Bikes.Any())
            {
                IEnumerable<NewLaunchedBikeEntityBase> newLaunchesList = objData.Bikes;
                var versionMinSpecs = SpecsFeaturesServiceGateway.GetVersionsMinSpecs(newLaunchesList.Select(m => m.VersionId)).GetEnumerator();
                VersionMinSpecsEntity minSpecs;
                foreach (var bike in newLaunchesList)
                {
                    if (versionMinSpecs.MoveNext())
                    {
                        minSpecs = versionMinSpecs.Current;
                        bike.MinSpecsList = minSpecs.MinSpecsList;
                    }
                }
            }
            return objData;
        }
    }
}