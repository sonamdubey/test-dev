using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 10 Mar 2017
    /// Summary: Model to holf scooter's brands- topBrands and remaining brands
    /// </summary>
    public class BrandWidgetModel
    {
        private IEnumerable<BikeMakeEntityBase> _brands = null;
        public ushort TopCount { get; private set; }
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
        public BrandWidgetModel(ushort topCount, IBikeMakes<BikeMakeEntity, int> bikeMakes)
        {
            _bikeMakes = bikeMakes;
            TopCount = topCount;
        }
        public BrandWidgetVM GetData(EnumBikeType page)
        {

            BrandWidgetVM objData = new BrandWidgetVM();
            _brands = _bikeMakes.GetMakesByType(page);
            switch (page)
            {

                case EnumBikeType.New:

                    foreach (var make in _brands)
                    {
                        make.Href = String.Format("/{0}-bikes/", make.MaskingName);
                        make.Title = String.Format("{0} bikes", make.MakeName);
                    }
                    break;
                case EnumBikeType.Used:
                    break;
                case EnumBikeType.Dealer:
                    foreach (var make in _brands)
                    {
                        make.Href = String.Format("/{0}-dealer-showrooms-in-india/", make.MaskingName);
                        make.Title = String.Format("{0} dealer showrooms in India/", make.MakeName);
                    }
                    break;
                case EnumBikeType.ServiceCenter:
                    break;
                case EnumBikeType.Scooters:
                    _brands = _bikeMakes.GetScooterMakes();
                    foreach (var make in _brands)
                    {
                        make.Href = String.Format("/{0}-{1}/", make.MaskingName, !make.IsScooterOnly ? "scooters" : "bikes");
                        make.Title = String.Format("{0} scooters", make.MakeName);
                    }
                    break;
                default:
                    break;
            }
            objData.TopBrands = _brands.Take(TopCount);
            objData.OtherBrands = _brands.Skip(TopCount);
            return objData;
        }
    }
}