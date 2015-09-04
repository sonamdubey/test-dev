using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Entities.NewBikeSearch;
using System.Collections.Specialized;
using Bikewale.Notifications;
using Bikewale.Utility;
using System.Configuration;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.Pager;

namespace Bikewale.BAL.NewBikeSearch
{
    public class ProcessFilter : IProcessFilter
    {
        private InputBaseEntity _input = null;
        private FilterInput _filters = null;
        public ProcessFilter(InputBaseEntity inputFilter)
        {
            inputFilter = _input;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To process new search page filters
        /// </summary>
        /// <param name="objInput"></param>
        /// <returns></returns>
        public FilterInput ProcessFilters(InputBaseEntity objInput)
        {
            _filters = new FilterInput();
            try
            {
                if(!String.IsNullOrEmpty(_input.Bike))
                {
                    ProcessBike();
                }

                if(!String.IsNullOrEmpty(_input.Displacement))
                {
                    ProcessDisplacement();
                }

                if(!String.IsNullOrEmpty(_input.Budget))
                {
                    ProcessBudget();
                }

                if(!String.IsNullOrEmpty(_input.Mileage))
                {
                    ProcessMileage();
                }

                if(!String.IsNullOrEmpty(_input.RideStyle))
                {
                    ProcessRideStyle();
                }

                if(!String.IsNullOrEmpty(_input.StartType))
                {
                    ProcessStartType();
                }


                if(!String.IsNullOrEmpty(_input.AntiBreakingSystem))
                {
                    ProcessABS();
                }

                if(!String.IsNullOrEmpty(_input.BrakeType))
                {
                    ProcessBrakeType();
                }

                if(!String.IsNullOrEmpty(_input.AlloyWheel))
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
            catch(Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessFilters");
                objError.SendMail();
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
            IPager objPager;
            try
            {
                _input.PageSize = !String.IsNullOrEmpty(_input.PageSize) ? _input.PageSize : ConfigurationManager.AppSettings["PageSize"];

                using(IUnityContainer container=new UnityContainer())
                {
                    container.RegisterType<IPager, Bikewale.BAL.Pager.Pager>();
                    objPager = container.Resolve<IPager>();

                    objPager.GetStartEndIndex(Convert.ToInt32(_input.PageSize), Convert.ToInt32(_input.PageNo), out startIndex,out endIndex);

                    _filters.StartIndex = startIndex;
                    _filters.EndIndex = endIndex;
                }
            }
            catch(Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessPaging");
                objError.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To process alloy wheel filter
        /// </summary>
        private void ProcessAlloyWheel()
        {
            try
            {
                string[] wheelTypes = _input.AlloyWheel.Split(' ');
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
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessAlloyWheel");
                objError.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To process brake type filter
        /// </summary>
        private void ProcessBrakeType()
        {
            try
            {
                string[] brakeTypes = _input.BrakeType.Split(' ');
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
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessBrakeType");
                objError.SendMail();
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
                string[] ABS = _input.AntiBreakingSystem.Split(' ');
                foreach(string strABS in ABS)
                {
                    if (strABS == ((int)AntiBreakingSystem.ABSAvailable).ToString())
                        _filters.ABSAvailable = true;
                    else if (strABS == ((int)AntiBreakingSystem.ABSNotAvailable).ToString())
                        _filters.ABSNotAvailable = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessABS");
                objError.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Aug 2015
        /// Summary : To process start type filter
        /// </summary>
        private void ProcessStartType()
        {
            try
            {
                string[] startTypes = _input.StartType.Split(' ');
                foreach (string startType in startTypes)
                {
                    if (startType == ((int)StartType.Electric).ToString())
                        _filters.Electric = true;
                    else if (startType == ((int)StartType.Manual).ToString())
                        _filters.Manual = true;
                }
            }
            catch(Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessStartType");
                objError.SendMail();
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
                if(_input.RideStyle.Contains("3"))
                {
                    _input.RideStyle += " 4";
                }

                _filters.RideStyle = _input.RideStyle.Split(' ');
            }
            catch(Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessRideStyle");
                objError.SendMail();
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
                _filters.Mileage = _input.Mileage.Split('-');
            }
            catch(Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessMileage");
                objError.SendMail();
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
            catch(Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessDisplacement");
                objError.SendMail();
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
                if(Validate.ValidateNumericRange(_input.Budget))
                {
                    string[] budgetRange = _input.Budget.Split('-');

                    if(!String.IsNullOrEmpty(budgetRange[0]) && !String.IsNullOrEmpty(budgetRange[1]))
                    {
                        _filters.MinBudget = budgetRange[0];
                        _filters.MaxBudget=budgetRange[1];
                    }
                    else if(!String.IsNullOrEmpty(budgetRange[0]) && String.IsNullOrEmpty(budgetRange[1]))
                    {
                        _filters.MinBudget=budgetRange[0];
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessBudget");
                objError.SendMail();
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

                foreach(string str in bike)
                {
                    if(str.Contains('.'))
                    {
                        model = str.Split('.')[1] + " ";
                    }
                    else
                    {
                        make = str + " ";
                    }
                }

                _filters.Make = make.Split(' ');
                _filters.Make = _filters.Make.Take<string>(_filters.Make.Length - 1).ToArray<string>();

                _filters.Model = model.Split(' ');
                _filters.Model = _filters.Model.Take<string>(_filters.Model.Length - 1).ToArray<string>();
            }
            catch(Exception ex)
            {
                ErrorClass objError = new ErrorClass(ex, "Bikewale.BAL.NewBikeSearch.ProcessFilter.ProcessBike");
                objError.SendMail();
            }
        }
    }   //End of class
}   //End of namespace
