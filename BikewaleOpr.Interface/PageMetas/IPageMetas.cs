namespace BikewaleOpr.Interface
{
    /// <summary>
    /// Interface for Page metas BAL
    /// </summary>
    public interface IPageMetas
    {
        bool UpdatePageMetaStatus(string id, ushort status);
    }
}
