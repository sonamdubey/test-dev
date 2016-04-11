using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.JSErrorLog
{
    /// <summary>
    /// Author : Sushil Kumar
    /// Created On : 10th March 2016
    /// Description : Entity used for logging javascript exception/error to bikewalebugs@gmail.com
    /// </summary>
    public class JSExceptionEntity
    {
        public string Message { get; set; }
        public string ErrorType { get; set; }
        public string SourceFile { get; set; }
        public string LineNo { get; set; }
        public string Trace { get; set; }
    }
}
