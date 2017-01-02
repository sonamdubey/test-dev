
namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Dec 2016
    /// Description :   Call To Action Entity Base
    /// </summary>
    public class CallToActionEntityBase
    {
        public ushort Id { get; set; }
        public string DisplayTextLarge { get; set; }
        public string DisplayTextSmall { get; set; }
        public string Display { get { return string.Format("{0}(Desktop) - {1}(Mobile)", this.DisplayTextLarge, this.DisplayTextSmall); } }
    }
}
