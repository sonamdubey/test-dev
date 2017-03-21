
using BikewaleOpr.Entity;
namespace BikewaleOpr.Interface
{
    /// <summary>
    /// Created by : Sajal Gupta on 21-03-2017
    /// Description : Interface for home page of opr
    /// </summary>
    public interface IHomePage
    {
        HomePageData GetHomePageData(string id, string userName);
    }
}
