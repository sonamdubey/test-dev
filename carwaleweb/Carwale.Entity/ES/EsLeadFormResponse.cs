
namespace Carwale.Entity.ES
{
    public class EsLeadFormResponse
    {
        public int Id { get; set; }
        public int LeadTypeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Power { get; set; }
        public int CityId { get; set; }
        public int Age { get; set; }
        public string Profession  { get; set; }
        public string CurrentCar { get; set; }
        public bool Licence { set; get; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkdIn { set; get; }
        public string Instagram { set; get; }
    }
}
