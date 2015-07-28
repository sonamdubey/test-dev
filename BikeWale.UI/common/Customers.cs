using System;
using System.Web;

namespace Bikewale.Common
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 7 Nov 2012
    /// Summary : This class will have properties only. This class can be used to store and retrieve the customer details.
    /// </summary>
    public class Customers
    {
        /// <summary>
        ///     Customer Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Name of the customer
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Email id of the customer
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Mobile no of the customer
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        ///     State Id of the customer
        /// </summary>
        public string StateId { get; set; }

        /// <summary>
        ///     State of the customer
        /// </summary>
        public string State { get; set; }

        /// <summary>
        ///     City Id of the customer
        /// </summary>
        public string CityId { get; set; }

        /// <summary>
        ///     City of the customer
        /// </summary>
        public string City { get; set; }

        /// <summary>
        ///     Address of the customer
        /// </summary>
        public string Adress { get; set; }

        /// <summary>
        ///  Phone of the customer
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///     Customer is mobile verified or not.
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        ///     Password salt of the custome
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        ///     Password hash of the customer
        /// </summary>
        public string Hash { get; set; }

    }   // End of class
}   // End of namespace