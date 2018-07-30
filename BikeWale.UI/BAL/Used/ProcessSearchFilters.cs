using Bikewale.Entities.Used.Search;
using Bikewale.Interfaces.Used.Search;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;

namespace Bikewale.BAL.Used.Search
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 10 sept 2016
    /// summary : Class have functions to process the raw used bikes search filters and copy them into usable prcessedinputfilters class.
    /// </summary>
    public class ProcessSearchFilters : ISearchFilters
    {
        ProcessedInputFilters objProcessedFilters = null;
        InputFilters objInputFilters = null;

        /// <summary>
        /// Function to process all filters and populate the data into output entity
        /// </summary>
        /// <param name="objFilters">Input raw filters data</param>
        /// <returns>Returns processed filters into ProcessedInputFilters entity</returns>
        public ProcessedInputFilters ProcessFilters(InputFilters objFilters)
        {
            objInputFilters = objFilters;
            objProcessedFilters = new ProcessedInputFilters();

            try
            {
                objProcessedFilters.CityId = objInputFilters.City;

                if (!string.IsNullOrEmpty(objInputFilters.Make))
                    ProcessMakes();

                if (!string.IsNullOrEmpty(objInputFilters.Model))
                    ProcessModels();

                if (!string.IsNullOrEmpty(objInputFilters.Budget))
                    ProcessBudget();

                if (!string.IsNullOrEmpty(objInputFilters.Kms))
                    ProcessKilometers();

                if (!string.IsNullOrEmpty(objInputFilters.Age))
                    ProcessBikeAge();

                if (!string.IsNullOrEmpty(objInputFilters.Owner))
                    ProcessOwners();

                if (!string.IsNullOrEmpty(objInputFilters.ST))
                    ProcessSellers();

                objProcessedFilters.SortOrder = objInputFilters.SO;

                ProcessPaging();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.ProcessFilters");
                
            }

            return objProcessedFilters;
        }


        /// <summary>
        /// Function to filter the makes list and populate it into array
        /// </summary>
        private void ProcessMakes()
        {
            try
            {
                objProcessedFilters.Make = objInputFilters.Make.Split('+');        
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.ProcessMakes");
                
            }
        }

        /// <summary>
        /// Function to filter the models list and populate it into array
        /// </summary>
        private void ProcessModels()
        {
            try
            {
                objProcessedFilters.Model = objInputFilters.Model.Split('+');
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.ProcessModels");
                
            }
        }

        /// <summary>
        /// Function to get the min and max budget
        /// </summary>
        private void ProcessBudget()
        {
            try
            {
                if (CommonValidators.ValidateNumericRange(objInputFilters.Budget, '+'))
                {
                    string[] budgetRange = objInputFilters.Budget.Split('+');

                    objProcessedFilters.MinBudget = budgetRange[0];
                    objProcessedFilters.MaxBudget = budgetRange[1];
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.ProcessBudget");
                
            }
        }

        /// <summary>
        /// Process the kilometers data and populate in array
        /// </summary>
        private void ProcessKilometers()
        {
            try
            {
                if (CommonValidators.IsValidNumber(objInputFilters.Kms))
                {
                    objProcessedFilters.Kms = objInputFilters.Kms;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.ProcessKilometers");
                
            }
        }

        /// <summary>
        /// Process bikes age parameters
        /// </summary>
        private void ProcessBikeAge()
        {
            try
            {
                if (CommonValidators.IsValidNumber(objInputFilters.Age))
                {                                         
                    objProcessedFilters.Age = objInputFilters.Age;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.ProcessBikeAge");
                
            }
        }

        /// <summary>
        /// Process the number of owners data e.g. data is 0+1+2+3+4
        /// </summary>
        private void ProcessOwners()
        {
            try
            {
                objProcessedFilters.Owners = objInputFilters.Owner.Split('+');
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.ProcessSellers");
                
            }
        }

        /// <summary>
        /// Process type of sellers
        /// </summary>
        private void ProcessSellers()
        {
            try
            {
                objProcessedFilters.SellerTypes = objInputFilters.ST.Split('+');
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.ProcessSellers");
                
            }
        }

        /// <summary>
        /// Function to process the page filters
        /// </summary>
        private void ProcessPaging()
        {
            int startIndex = 0, endIndex = 0;
            try
            {
                // If page no is not valid, then consider this as a first page
                objInputFilters.PN = objInputFilters.PN <= 0 ? 1 : objInputFilters.PN;

                // If page size is not passed then take the default page size
                objInputFilters.PS = objInputFilters.PS <= 0 ? Convert.ToInt32(Utility.BWConfiguration.Instance.PageSize) : objInputFilters.PS;

                using (IUnityContainer container = new UnityContainer())
                {
                    Paging.GetStartEndIndex(objInputFilters.PS, objInputFilters.PN, out startIndex, out endIndex);

                    objProcessedFilters.StartIndex = startIndex;
                    objProcessedFilters.EndIndex = endIndex;
                    objProcessedFilters.PageNo = objInputFilters.PN;
                    objProcessedFilters.PageSize = objInputFilters.PS;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Used.ProcessPaging");
                
            }
        }

    }   // Class
}   // namespace
