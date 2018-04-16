using Bikewale.Entities.NewBikeSearch;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Linq;

namespace Bikewale.DAL.NewBikeSearch
{
    public class ProcessFilter : IProcessFilter
    {
        private InputBaseEntity _input = null;
        private FilterInput _filters = null;

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To process new search page filters
        /// Modified by : Vivek Singh Tomar on 10th Oct 2017
        /// Summary : Added processing of AntiBreakingSystem
        /// </summary>
        /// <param name="objInput"></param>
        /// <returns></returns>
        public FilterInput ProcessFilters(InputBaseEntity objInput)
        {
            _input = objInput;
            _filters = new FilterInput();
            try
            {
                if (!String.IsNullOrEmpty(_input.Bike))
                {
                    ProcessBike();
                }

                if (!String.IsNullOrEmpty(_input.Displacement))
                {
                    ProcessDisplacement();
                }

                if (!String.IsNullOrEmpty(_input.Budget))
                {
                    ProcessBudget();
                }

                if (!String.IsNullOrEmpty(_input.Mileage))
                {
                    ProcessMileage();
                }

                if (!String.IsNullOrEmpty(_input.RideStyle))
                {
                    ProcessRideStyle();
                }

                if (!String.IsNullOrEmpty(_input.StartType))
                {
                    ProcessStartType();
                }


                if (!String.IsNullOrEmpty(_input.ABS))
                {
                    ProcessABS();
                }

                if (!String.IsNullOrEmpty(_input.AntiBreakingSystem))
                {
                    ProcessAntiBreakingSystem();
                }

                if (!String.IsNullOrEmpty(_input.BrakeType))
                {
                    ProcessBrakeType();
                }

                if (!String.IsNullOrEmpty(_input.AlloyWheel))
                {
                    ProcessAlloyWheel();
                }

                _input.PageNo = !String.IsNullOrEmpty(_input.PageNo) ? _input.PageNo : "1";

                if (!String.IsNullOrEmpty(_input.PageNo))
                {
                    ProcessPaging();
                }

                if (!String.IsNullOrEmpty(_input.sc) && _input.sc != "-1")
                    _filters.sc = _input.sc;

                if (!String.IsNullOrEmpty(_input.so) && _input.so != "-1")
                    _filters.so = _input.so;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessFilters");

            }
            return _filters;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To process paging filters
        /// </summary>
        private void ProcessPaging()
        {
            int startIndex = 0, endIndex = 0;
            try
            {
                _input.PageSize = !String.IsNullOrEmpty(_input.PageSize) ? _input.PageSize : ConfigurationManager.AppSettings["PageSize"];

                using (IUnityContainer container = new UnityContainer())
                {
                    Paging.GetStartEndIndex(Convert.ToInt32(_input.PageSize), Convert.ToInt32(_input.PageNo), out startIndex, out endIndex);

                    _filters.StartIndex = startIndex;
                    _filters.EndIndex = endIndex;
                    _filters.PageSize = _input.PageSize;
                    _filters.PageNo = _input.PageNo;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessPaging");

            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To process alloy wheel filter
        /// Modified by : Snehal Dange on 11th April 2018
        /// Desc : Added wheel types array to filters
        /// </summary>
        private void ProcessAlloyWheel()
        {
            try
            {
                string[] wheelTypes = _input.AlloyWheel.Split(' ');
                if (wheelTypes != null && wheelTypes.Any())
                {
                    _filters.Wheels = wheelTypes;
                }
                foreach (string wheelType in wheelTypes)
                {
                    if (wheelType == ((int)WheelType.Alloy).ToString())
                        _filters.SpokeWheel = true;
                    else if (wheelType == ((int)WheelType.Spoke).ToString())
                        _filters.AlloyWheel = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessAlloyWheel");

            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To process brake type filter
        /// Modified by : Snehal Dange on 11th April 2018
        /// Desc : added brakes values in Brakes array
        /// </summary>
        private void ProcessBrakeType()
        {
            try
            {
                string[] brakeTypes = _input.BrakeType.Split(' ');
                if (brakeTypes != null && brakeTypes.Any())
                {
                    _filters.Brakes = brakeTypes;
                }
                foreach (string brakeType in brakeTypes)
                {
                    if (brakeType == ((int)Brake.Disc).ToString())
                        _filters.DiscBrake = true;
                    else if (brakeType == ((int)Brake.Drum).ToString())
                        _filters.DrumBrake = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessBrakeType");

            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To process Anti Breaking System filter
        /// </summary>
        private void ProcessABS()
        {
            try
            {
                string[] ABS = _input.ABS.Split(' ');
                foreach (string strABS in ABS)
                {
                    if (strABS == ((int)AntiBreakingSystem.ABSAvailable).ToString())
                        _filters.ABSAvailable = true;
                    else if (strABS == ((int)AntiBreakingSystem.ABSNotAvailable).ToString())
                        _filters.ABSNotAvailable = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessABS");

            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 10th Oct 2017
        /// Summary : Filter AntiBreakingSystem
        /// </summary>
        private void ProcessAntiBreakingSystem()
        {
            try
            {
                string[] ABS = _input.AntiBreakingSystem.Split(' ');
                foreach (string strABS in ABS)
                {
                    if (strABS == ((int)AntiBreakingSystem.ABSAvailable).ToString())
                        _filters.ABSAvailable = true;
                    else if (strABS == ((int)AntiBreakingSystem.ABSNotAvailable).ToString())
                        _filters.ABSNotAvailable = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessAntiBreakingSystem");

            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To process start type filter
        /// Modified by : Snehal Dange on 11th April 2018
        /// Desc : Added startTypes array values to filters
        /// </summary>
        private void ProcessStartType()
        {
            try
            {
                string[] startTypes = _input.StartType.Split(' ');
                if (startTypes != null && startTypes.Any())
                {
                    _filters.StartType = startTypes;
                }
                foreach (string startType in startTypes)
                {
                    if (startType == ((int)StartType.Electric).ToString())
                        _filters.Electric = true;
                    else if (startType == ((int)StartType.Manual).ToString())
                        _filters.Manual = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessStartType");

            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To process ride style filter
        /// </summary>
        private void ProcessRideStyle()
        {
            try
            {
                if (_input.RideStyle.Contains("3"))
                {
                    _input.RideStyle += " 4";
                }

                _filters.RideStyle = _input.RideStyle.Split(' ');
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessRideStyle");

            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To process displacement Filter
        /// </summary>
        private void ProcessMileage()
        {
            try
            {
                _filters.Mileage = _input.Mileage.Split(' ');
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessMileage");

            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To process displacement Filter
        /// </summary>
        private void ProcessDisplacement()
        {
            try
            {
                _filters.Displacement = _input.Displacement.Split(' ');
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessDisplacement");

            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To process Budget Filter
        /// </summary>
        private void ProcessBudget()
        {
            try
            {
                if (CommonValidators.ValidateNumericRange(_input.Budget))
                {
                    string[] budgetRange = _input.Budget.Split('-');

                    if (!String.IsNullOrEmpty(budgetRange[0]) && !String.IsNullOrEmpty(budgetRange[1]))
                    {
                        _filters.MinBudget = budgetRange[0];
                        _filters.MaxBudget = budgetRange[1];
                    }
                    else if (!String.IsNullOrEmpty(budgetRange[0]) && String.IsNullOrEmpty(budgetRange[1]))
                    {
                        _filters.MinBudget = budgetRange[0];
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessBudget");

            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To process Make and Model Filters
        /// </summary>
        private void ProcessBike()
        {
            try
            {
                string[] bike = _input.Bike.Split(' ');
                string make = string.Empty, model = string.Empty;

                foreach (string str in bike)
                {
                    if (str.Contains('.'))
                    {
                        model += str.Split('.')[1] + " ";
                    }
                    else
                    {
                        make += str + " ";
                    }
                }

                _filters.Make = make.Split(' ');
                _filters.Make = _filters.Make.Take<string>(_filters.Make.Length - 1).ToArray<string>();

                _filters.Model = model.Split(' ');
                _filters.Model = _filters.Model.Take<string>(_filters.Model.Length - 1).ToArray<string>();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessBike");

            }
        }
    }   //End of class
}   //End of namespace
