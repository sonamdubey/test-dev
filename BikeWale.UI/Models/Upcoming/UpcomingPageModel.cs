using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 07-Apr-2017
    /// summary: Upcoming bikes page model
    /// </summary>
    public class UpcomingPageModel
    {
        #region Private variables

        private IUpcoming _upcoming = null;
        private EnumUpcomingBikesFilter filter = EnumUpcomingBikesFilter.Default;

        #endregion

        #region Constructor

        public UpcomingPageModel(IUpcoming upcoming)
        {
            _upcoming = upcoming;
        }
        #endregion

        #region Public members

        public UpcomingBikesListInputEntity Filters { get; set; }
        public EnumUpcomingBikesFilter SortBy { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 07-Apr-2017 
        /// </returns>
        public UpcomingPageVM GetData()
        {
            UpcomingPageVM objUpcoming = new UpcomingPageVM();
            try
            {
                var upcomingBikes = _upcoming.GetModels(Filters, SortBy);
                BindMakes(upcomingBikes, 6);
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.UpcomingPageModel.GetData");
            }
            return objUpcoming;
        }

        private BrandWidgetVM BindMakes(IEnumerable<UpcomingBikeEntity> upcomingBikes, uint topCount)
        {
            topCount = 6;
            BrandWidgetVM brands = new BrandWidgetVM();
            ICollection<BikeMakeEntityBase> topBrands = new List<BikeMakeEntityBase>();
            ICollection<BikeMakeEntityBase> otherBrands = new List<BikeMakeEntityBase>();
            var makes = upcomingBikes.Select(x => x.MakeBase);
            int i = 0;
            foreach (var make in makes)
            {
                if (i < topCount)
                {
                    topBrands.Add(new BikeMakeEntityBase()
                    {
                        MakeId = make.MakeId,
                        MakeName = make.MakeName
                    });
                }
                else
                {
                    otherBrands.Add(new BikeMakeEntityBase()
                    {
                        MakeId = make.MakeId,
                        MakeName = make.MakeName
                    });
                }
            }
            brands.TopBrands = topBrands;
            brands.OtherBrands = otherBrands;
            return brands;
        }

        #endregion
    }
}
