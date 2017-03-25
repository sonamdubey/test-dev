using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.NewLaunched;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 25-Mar-2017
    /// Summary: Widget for new lauched bikes on pages . For ex. Homepage
    /// </summary>
    public class NewLaunchedWidgetModel
    {
        public IEnumerable<NewLaunchedBikeEntityBase> Bikes { get; set; }
        private INewBikeLaunchesBL _newLaunches { get; set; }
        private ushort _recordCount;
        public NewLaunchedWidgetModel(ushort recodCount, INewBikeLaunchesBL newLaunches)
        {
            _newLaunches = newLaunches;
            _recordCount = recodCount;
        }
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
        /// </returns>
        public NewLaunchedWidgetVM GetData()
        {
            NewLaunchedWidgetVM objData = new NewLaunchedWidgetVM();
            var objFilters = new InputFilter()
            {
                PageNo = 1,
                PageSize = 9
            };
            var bikeBase = _newLaunches.GetBikes(objFilters);
            objData.Bikes = bikeBase.Bikes;
            return objData;
        }
    }
}