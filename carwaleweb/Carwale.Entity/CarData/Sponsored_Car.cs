using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class Sponsored_Car
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsSponsored { get; set; }
        public int CampaignCategoryId { get; set; }
        public int CategorySection { get; set; }
        public int PlatformId { get; set; }

        public string Ad_Html { get; set; }
        public string SponsoredTitle { get; set; }
        public string ImageUrl { get; set; }
        public string JumbotronPosition { get; set; }
        public string VerticalPosition { get; set; }
        public string HorizontalPosition { get; set; }
        public string WidgetPosition { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDafault { get; set; }
        public string BackgroundColor { get; set; }

        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public string MakeName { get; set; }
        public string ModelMaskingName { get; set; }
        public string ModelName { get; set; }
        public string Subtitle { get; set; }
        public string CardHeader { get; set; }
        public List<int> Postion { get; set; }
        public List<Sponsored_CarLink> Links { get; set; }        
    }

    [Serializable]
    public class Sponsored_CarLink
    {
        public string Name { get; set; }
        public bool IsInsideApp { get; set; }
        public string Url { get; set; }
        public bool IsUpcoming { get; set; }
        public object PayLoad { get; set; }
    }
}
