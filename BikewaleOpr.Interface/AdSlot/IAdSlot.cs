namespace BikewaleOpr.Interface.AdSlot
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 31 Oct 2017
    /// Description : Provide BAL methods for ad slots.
    /// </summary>
    public interface IAdSlot
    {
        bool ChangeStatus(uint AdId, int UserId);
    }
}
