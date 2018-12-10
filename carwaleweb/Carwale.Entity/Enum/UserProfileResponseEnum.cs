using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Enum
{
    /// Written by Abhsihek Lovanshi on 19 July, 2018.
    /// <summary> 
    /// This is enum for user-profile-response.
    /// It maintains order of admax responses(order in which response is recieved from admax request)
    /// Order of this enum must be maintained in same order as requested in admax request.
    /// This enum must be updated if there is any update in user-profile-request(admax request).
    /// If this enum is not updated then it will throw error
    /// </summary>
    public enum UserProfileResponseEnum
    {
        /// <summary>
        /// It is for car type e.g. "New","Used" etc.
        /// </summary>
        /// <remarks> The value must be string</remarks>
        CarType,
        /// <summary>
        /// It is  for lead count (Number of times person had filled leads)
        /// </summary>
        /// <remarks> The value must be numeric</remarks>
        LeadCount,
        /// <summary>
        /// It is for list of models viewed recently e.g. "tiago","ertiga" etc.
        /// </summary>
        /// <remarks> The value must be string</remarks>
        ModelsList,
        /// <summary>
        /// It is for body style e.g. "HatchBack","sedan" etc.
        /// </summary>
        /// <remarks> The value must be string</remarks>
        BodyStyleList,
        /// <summary>
        /// It is for budget segment list e.g. "BP"
        /// </summary>
        /// <remarks> The value must be string</remarks>
        BudgetSegmentList,
        /// <summary>
        /// It is for car count (Number of cars viewed)
        /// </summary>
        /// <remarks> The value must be numeric</remarks>
        CarCount,

        // <summary>
        // It is for leadPrediction and this is not dependent on sequence 
        //</summary>
        // <remarks> The value must be string</remarks>
        LeadPrediction
    }
}
