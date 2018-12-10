using Carwale.Entity.Enum;
using Carwale.Entity.Price;
using Carwale.Interfaces.Prices;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Carwale.BL.Prices
{
    public class EmiCalculatorBl : IEmiCalculatorBl
    {
        private static readonly int _exShowroomPercentForDefaultDownPayment = CustomParser.parseIntObject(ConfigurationManager.AppSettings["ExShowroomPercentForDefaultDownPayment"]);

        /// <summary>
        /// This function calculates the down payment default value
        /// </summary>
        /// <param name="downPaymentMinValue">Down payment minimum value implies On-road price minus exShowroom price</param>
        /// <param name="exShowroom">Ex-Showroom price</param>
        /// <returns></returns>
        public int CalculateDownPaymentDefaultValue(int downPaymentMinValue, int exShowroom)
        {
            int downPaymentDefaultValue = downPaymentMinValue + (exShowroom * _exShowroomPercentForDefaultDownPayment) / 100;
            return downPaymentDefaultValue;
        }

        public Tuple<int, int> GetDownPaymentValues(List<ChargeGroupPrice> compulsoryPrices)
        {
            int minAmountToPay = 0;
            int exShowroomPrice = 0;
            for (int item = 0; item < compulsoryPrices.Count; item++)
            {
                var chargeGroup = compulsoryPrices[item];
                for (int chargePrice = 0; chargePrice < chargeGroup.ChargePrice.Count; chargePrice++)
                {
                    if (chargeGroup.ChargePrice[chargePrice].Charge.Id != (int)PricesCategoryItem.Exshowroom)
                    {
                        minAmountToPay = minAmountToPay + chargeGroup.ChargePrice[chargePrice].Price;
                    }
                    else
                    {
                        exShowroomPrice = chargeGroup.ChargePrice[chargePrice].Price;
                    }
                }
            }
            return new Tuple<int, int>(minAmountToPay, exShowroomPrice);
        }
    }
}
