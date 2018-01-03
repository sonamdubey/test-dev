
using BikewaleOpr.Interface.AdOperation;
namespace BikewaleOpr.Models
{
    /// <summary>
    /// Created by : Snehal Dange on 2nd Jan 2018
    /// Description: Model created to manage ad-operations
    /// </summary>
    public class AdOperation
    {
        public readonly IAdOperation _obj = null;
        public AdOperation(IAdOperation obj)
        {
            _obj = obj;
        }

        public AdOperationVM GetData()
        {
            AdOperationVM obj = null;

            return obj;

        }
    }
}