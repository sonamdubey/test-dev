
using System;
namespace BikewaleOpr.Entity.Users
{
    [Serializable]
    public class User
    {
        public string Email { get; set; }
        public uint Id { get; set; }
        public string Name { get; set; }
    }
}
