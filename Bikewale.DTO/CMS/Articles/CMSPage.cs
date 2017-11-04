namespace Bikewale.DTO.CMS.Articles
{
    public class CMSPage
    {
        public ulong pageId { get; set; }
        public ushort Priority { get; set; }
        public string PageName { get; set; }
        public string Content { get; set; }

        public HtmlContent htmlContent { get; set; }
    }
}
