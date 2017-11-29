namespace Bikewale.Service.Controllers.Customer
{
    public class RegisterInputParameters
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }         
        public string Password { get; set; }
        public string ClientIP { get; set; }
    }
}
