using Carwale.Entity.Elastic;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Utility.Classified;
using Nest;
using System;
using System.Linq;

namespace Carwale.DAL.Classified.ElasticSearch
{
    public class QueryContainerRepository<T> : IQueryContainerRepository<T> where T: class
    {
        public QueryContainer GetCommonQueryContainerForSearchPage(
            ElasticOuptputs filterInputs
            , string carsWithPhotos
            , QueryContainerDescriptor<T> queryContainerDescriptor)
        {
            return CreateQuery(filterInputs, carsWithPhotos, queryContainerDescriptor, false);
        }

        public QueryContainer GetCommonQueryContainerForSearchPage(
            ElasticOuptputs filterInputs
            , string carsWithPhotos
            , QueryContainerDescriptor<T> queryContainerDescriptor
            , bool isNearByCity)
        {
            return CreateQuery(filterInputs, carsWithPhotos, queryContainerDescriptor, isNearByCity);
        }

        private static QueryContainer CreateQuery(
            ElasticOuptputs filterInputs
            , string carsWithPhotos
            , QueryContainerDescriptor<T> queryContainerDescriptor
            , bool isNearByCity)
        {

            double photoCount;
            double.TryParse(carsWithPhotos, out photoCount);
            return queryContainerDescriptor.Bool(b => b
               .Filter(ff => ff
                  .Bool(bb => bb
                     .Must(mm =>
                     {
                         QueryContainer bf =
                             mm.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));

                         if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                             bf |= mm.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                         else
                             bf &= mm.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                         if (!isNearByCity)
                         {
                             if (filterInputs.cities == null || (filterInputs.cities.Length > 0 && filterInputs.cities[0] == Constants.AllIndiaCityId))
                                 bf &= !mm.Term("packageType", Constants.DiamondDealerPackageType); // diamond dealer packages are excluded for all india
                             else
                                 bf &= mm.Terms(terms => terms.Field("cityIds").Terms<string>(filterInputs.cities)); 
                         }

                         QueryContainer sellerQc = mm.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                         if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                         {
                             sellerQc &= !mm.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                             sellerQc |= mm.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                         }


                         bf &= sellerQc && mm.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                             mm.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                             mm.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                             mm.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                             mm.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                             mm.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                 .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                             mm.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                 .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                             mm.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                 .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));

                         if (filterInputs.IsCarTradeCertifiedCars)
                         {
                             bf &= mm.Term("certificationId", Constants.CarTradeCertificationId);
                         }

                         if (filterInputs.IsFranchiseCars)
                         {
                             bf &= mm.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                         }

                         bf &= mm.Range(y => y.Field("photoCount").GreaterThanOrEquals(photoCount));
                         return bf;
                     })
                  )
               ));
        }
    }
}
