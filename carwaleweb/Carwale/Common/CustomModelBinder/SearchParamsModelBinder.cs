using Carwale.Entity.Classified.Search;
using System;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.Common.CustomModelBinder
{
    public class SearchParamsModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                object model = base.BindModel(controllerContext, bindingContext);
                var searchParams = model as SearchParams;
                if (searchParams == null)
                {
                    return model;
                }
                SetValuesFromQs(searchParams, controllerContext.HttpContext.Request);
                IncrementFilterCount(searchParams);
                IncrementFilterCountForSlider(searchParams);
                return searchParams;
            }
            catch (Exception)
            {
                return base.BindModel(controllerContext, bindingContext);
            }
        }

        private void SetValuesFromQs(SearchParams searchParams, HttpRequestBase request)
        {
            searchParams.FilterByAdditional = request.QueryString["filterbyadditional"]?.Replace(',',' ');
            searchParams.Fuel = request.QueryString["fuel"]?.Replace(',', ' ');
            searchParams.Owners = request.QueryString["owners"]?.Replace(',', ' ');
            searchParams.BodyType = request.QueryString["bodytype"]?.Replace(',', ' ');
            searchParams.Trans = request.QueryString["trans"]?.Replace(',', ' ');
        }

        private static void IncrementFilterCount(SearchParams searchParams)
        {
            if (!string.IsNullOrWhiteSpace(searchParams.Car))
            {
                searchParams.FilterAppliedCount += SplitAndCount(searchParams.Car, ' ');
            }
            if (string.IsNullOrWhiteSpace(searchParams.Car) && searchParams.Make > 0) //for cases like audi-cars-in-mumbai
            {
                searchParams.FilterAppliedCount++;
            }
            if (!string.IsNullOrWhiteSpace(searchParams.Fuel))
            {
                searchParams.FilterAppliedCount += SplitAndCount(searchParams.Fuel, ' ');
            }
            if (!string.IsNullOrWhiteSpace(searchParams.BodyType))
            {
                searchParams.FilterAppliedCount += SplitAndCount(searchParams.BodyType, ' ');
            }
            if (!string.IsNullOrWhiteSpace(searchParams.Trans))
            {
                searchParams.FilterAppliedCount += SplitAndCount(searchParams.Trans, ' ');
            }
            if (!string.IsNullOrWhiteSpace(searchParams.Owners))
            {
                searchParams.FilterAppliedCount += SplitAndCount(searchParams.Owners, ' ');
            }
            if (!string.IsNullOrWhiteSpace(searchParams.FilterByAdditional))
            {
                searchParams.FilterAppliedCount += SplitAndCount(searchParams.FilterByAdditional, ' ');
            }
        }
        private static void IncrementFilterCountForSlider(SearchParams searchParams)
        {
            if (!string.IsNullOrWhiteSpace(searchParams.Year) && searchParams.Year.Contains("-"))
            {
                searchParams.FilterAppliedCount += SplitAndCountForSliders(searchParams.Year, '-');
            }
            if (!string.IsNullOrWhiteSpace(searchParams.Budget) && searchParams.Budget.Contains("-"))
            {
                searchParams.FilterAppliedCount += SplitAndCountForSliders(searchParams.Budget, '-');
            }
            if (!string.IsNullOrWhiteSpace(searchParams.Kms) && searchParams.Kms.Contains("-"))
            {
                searchParams.FilterAppliedCount += SplitAndCountForSliders(searchParams.Kms, '-');
            }
        }
        private static int SplitAndCount(string input, char delimiter)
        {
            return input.Split(delimiter).Length;
        }

        private static int SplitAndCountForSliders(string input, char delimiter)
        {
            string[] splitedArray = input.Split(delimiter);
            int filterAppliedCount = 0;
            if (!(splitedArray[0] == "0" && string.IsNullOrWhiteSpace(splitedArray[1]))) //not increment count for cases like budget=0-
            {
                filterAppliedCount++;
            }
            return filterAppliedCount;
        }
    }
}