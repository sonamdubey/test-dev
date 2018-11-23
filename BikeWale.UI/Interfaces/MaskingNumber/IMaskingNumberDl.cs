using Bikewale.Entities.MaskingNumber;

namespace Bikewale.Interfaces.MaskingNumber
{
    /// <summary>
    /// Author  : Kartik Rathod on 19 nov 2019
    /// Desc    : save ds es and masking number
    /// </summary>
    public interface IMaskingNumberDl
    {
        uint SaveMaskingNumberLead(MaskingNumberLeadEntity objLead);
    }
}