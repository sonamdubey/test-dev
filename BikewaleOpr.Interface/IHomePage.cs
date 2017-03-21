
using BikewaleOpr.Entity;
namespace BikewaleOpr.Interface
{
    public interface IHomePage
    {
        HomePageData GetHomePageData(string id, string userName);
    }
}
