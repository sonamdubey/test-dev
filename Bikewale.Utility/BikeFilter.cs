using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created by  :   Sushil Kumar on 23rd Nov 2017
    /// Description :   Class to filter bikes based on categories and flags
    /// </summary>
    public static class BikeFilter
    {
        /// <summary>
        /// Created by  :   Sushil Kumar on 23rd Nov 2017
        /// Description :   To filter makes based on category inorder to find similar makes to the specified one
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="makes"></param>
        /// <returns></returns>
        public static IEnumerable<BikeMakeEntityBase> FilterMakesByCategory(uint makeId, IEnumerable<BikeMakeEntityBase> makes)
        {
            List<BikeMakeEntityBase> popularBrandsList = null;
            IEnumerable<BikeMakeEntityBase> tempBrandsList = null;

            try
            {
                if (makeId > 0 && makes != null && makes.Any())
                {
                    ushort categoryId = 0;
                    BikeMakeEntityBase make = makes.FirstOrDefault(x => x.MakeId == makeId);
                    if (make != null)
                    {
                        categoryId = make.MakeCategoryId;
                    }
                    ushort[] arr;

                    switch (categoryId)
                    {
                        case 1: // commoner 1
                            arr = new ushort[] { 1, 2, 3, 4, 5 };
                            break;
                        case 2:// commoner 2
                            arr = new ushort[] { 2, 3, 4, 5, 1 };
                            break;
                        case 3: // premium 1
                            arr = new ushort[] { 3, 2, 4, 5, 1 };
                            break;
                        case 4: // premium 2
                            arr = new ushort[] { 4, 5, 3, 2, 1 };
                            break;
                        case 5: // scooters
                            arr = new ushort[] { 5, 4, 3, 2, 1 };
                            break;
                        default:
                            arr = new ushort[] { 1, 2, 3, 4, 5 };
                            break;
                    }

                    popularBrandsList = new List<BikeMakeEntityBase>();

                    tempBrandsList = makes.Where(x => x.MakeCategoryId == arr[0] && x.MakeId != makeId);

                    if (tempBrandsList != null)
                        popularBrandsList.AddRange(tempBrandsList.OrderBy(x => x.PopularityIndex));

                    tempBrandsList = makes.Where(x => x.MakeCategoryId == arr[1]);

                    if (tempBrandsList != null)
                        popularBrandsList.AddRange(tempBrandsList.OrderBy(x => x.PopularityIndex));

                    tempBrandsList = makes.Where(x => x.MakeCategoryId == arr[2]);

                    if (tempBrandsList != null)
                        popularBrandsList.AddRange(tempBrandsList.OrderBy(x => x.PopularityIndex));

                    tempBrandsList = makes.Where(x => x.MakeCategoryId == arr[3]);

                    if (tempBrandsList != null)
                        popularBrandsList.AddRange(tempBrandsList.OrderBy(x => x.PopularityIndex));

                    tempBrandsList = makes.Where(x => x.MakeCategoryId == arr[4]);

                    if (tempBrandsList != null)
                        popularBrandsList.AddRange(tempBrandsList.OrderBy(x => x.PopularityIndex));
                }
            }
            catch (System.Exception)
            {
                throw;
            }

            return popularBrandsList;
        }
    }
}
