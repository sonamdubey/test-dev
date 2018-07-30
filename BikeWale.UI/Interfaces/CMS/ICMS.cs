namespace Bikewale.Interfaces.CMS
{
    public interface ICMS
    {
        string GetArticleDetailsPage(uint basicId);
        string GetArticleDetailsPages(uint basicId);
    }
}
