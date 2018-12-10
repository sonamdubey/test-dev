using System;
using System.Collections.Generic;
using System.Web;
using System.Data;

/// <summary>
/// Interface fot Thread Details. Implemented by IThread .
/// </summary>
/// 
namespace Carwale.Interfaces.Forums
{
    public interface IThreadDetails
    {
        DataSet GetThreadDetails(int threadId, int startIndex, int endIndex);
    }// interface
}// namespace