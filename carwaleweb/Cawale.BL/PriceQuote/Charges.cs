using Carwale.Interfaces.PriceQuote;
using System.Collections.Generic;
using System;
using Carwale.Notifications.Logs;
using AutoMapper;
using Carwale.Entity.Price;

namespace Carwale.BL.PriceQuote
{
    public class Charges : ICharges
    {
        private readonly IChargesCacheRepository _chargesCache;
        public Charges(IChargesCacheRepository chargesCache)
        {
            _chargesCache = chargesCache;
        }

        public List<ChargeBase> GetComponents(int chargeId)
        {
            try
            {
                List<ChargeBase> chargeComponentDetails = new List<ChargeBase>();

                List<int> components = _chargesCache.GetComponents(chargeId);
                Dictionary<int, Charge> allCharges = _chargesCache.GetCharges();

                foreach (var componentId in components)
                {
                    if (allCharges.ContainsKey(componentId))
                    {
                        var chargeDetails = allCharges[componentId];
                        chargeComponentDetails.Add(Mapper.Map<ChargeBase>(chargeDetails));
                    }
                }

                if (chargeComponentDetails.Count > 0)
                {
                    return chargeComponentDetails;
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }

            return null;
        }
    }
}
