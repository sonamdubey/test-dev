using Carwale.Entity.CrossSell;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.CrossSell
{
    public class CrossSellOperations
    {
        public List<CrossSellDetail> PrioritizeAndFilterCrossSellList(List<CrossSellDetail> crossSells)
        {
            try
            {
                if (crossSells == null || crossSells.Count == 0)
                {
                    return null;
                }

                var minPriority = crossSells.Min(x => x.CampaignDetail.Priority);

                var crossSellMinPriorityList = from x in crossSells
                                               where x.CampaignDetail.Priority == minPriority
                                               select x;

                var crossSellList = crossSellMinPriorityList.ToList();

                if (crossSellList == null || crossSellList.Count == 0)
                {
                    return null;
                }

                ShuffleCrossSell(crossSellList);

                if (crossSellList == null || crossSellList.Count == 0)
                {
                    return null;
                }

                return crossSellList.GroupBy(x => x.CarVersionDetail.ModelId).Select(grp => grp.First()).ToList();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PrioritizeAndFilterCrossSellList");
                objErr.LogException();
                return null;
            }
        }
        public List<CrossSellDetail> PrioritizeCrossSellList(List<CrossSellDetail> crossSells)
        {
            try
            {
                if (crossSells == null || crossSells.Count == 0)
                {
                    return null;
                }

                var crossSellList = crossSells.OrderBy(x => x.CampaignDetail.Priority).ToList();

                return crossSellList.GroupBy(x => x.CarVersionDetail.ModelId).Select(grp => grp.First()).ToList();  //For Same Model Multiple cross sell campaigns can be available
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PrioritizeCrossSellList");
                objErr.LogException();
                return null;
            }
        }

        private void ShuffleCrossSell(List<CrossSellDetail> crossSells)
        {

            try
            {
                CrossSellDetail CrossSellCampaign;
                Random rnd = new Random();
                int totalCrossSells = crossSells.Count;
                while (totalCrossSells > 1)
                {
                    totalCrossSells--;
                    int randomIndex = rnd.Next(totalCrossSells + 1);
                    CrossSellCampaign = crossSells[randomIndex];
                    crossSells[randomIndex] = crossSells[totalCrossSells];
                    crossSells[totalCrossSells] = CrossSellCampaign;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ShuffleCrossSell");
                objErr.LogException();
            }
        }

        public CrossSellDetail GetRandomCrossSell(List<CrossSellDetail> crossSells)
        {
            try
            {

                if (crossSells == null || crossSells.Count == 0)
                    return null;

                var crossSellCount = crossSells.Count;
                Random rnd = new Random(DateTime.Now.Millisecond);
                int index = rnd.Next(crossSellCount);

                return crossSells[index];
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetRandomCrossSell");
                objErr.LogException();
                return null;
            }
        }
    }
}
