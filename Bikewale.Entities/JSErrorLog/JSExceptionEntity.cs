using System;
namespace Bikewale.Entities.JSErrorLog
{
    /// <summary>
    /// Author : Sushil Kumar
    /// Created On : 10th March 2016
    /// Description : Entity used for logging javascript exception/error to bikewalebugs@gmail.com
    /// Modified by: Dhruv Joshi
    /// Dated: 10th April 2018
    /// Details: Inherited Exception Class, changed property name from Message to Details
    /// </summary>
    public class JSExceptionEntity: Exception
    {
        public string Details { get; set; }
        public string ErrorType { get; set; }
        public string SourceFile { get; set; }
        public string LineNo { get; set; }
        public string Trace { get; set; }
    }
}
