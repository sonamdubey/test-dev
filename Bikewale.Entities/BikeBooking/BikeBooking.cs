using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeBooking
{
    public class BookingRequest
    {
        private Nullable<DateTime> _bookingDate;

        public Nullable<DateTime> BookingDate
        {
            get { return _bookingDate; }
            set { _bookingDate = value; }
        }
        private uint _branchId;

        public uint BranchId
        {
            get { return _branchId; }
            set { _branchId = value; }
        }

        private string _couponCode;

        public string CouponCode
        {
            get { return _couponCode; }
            set { _couponCode = value; }
        }

        private string _customerAddress;

        public string CustomerAddress
        {
            get { return _customerAddress; }
            set { _customerAddress = value; }
        }

        private uint _customerId;

        public uint CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; }
        }

        private uint _cwOfferId;

        public uint CWOfferId
        {
            get { return _cwOfferId; }
            set { _cwOfferId = value; }
        }
        private uint _inquiryId;

        public uint InquiryId
        {
            get { return _inquiryId; }
            set { _inquiryId = value; }
        }

        string _keyValue = string.Empty;
        public string Key
        {
            get { return _keyValue; }
            set { _keyValue = value; }
        }

        private uint _paymentAmount;

        public uint PaymentAmount
        {
            get { return _paymentAmount; }
            set { _paymentAmount = value; }
        }

        private string _paymentMode;

        public string PaymentMode
        {
            get { return _paymentMode; }
            set { _paymentMode = value; }
        }

        private Nullable<DateTime> _pickupDateTime;

        public Nullable<DateTime> PickupDateTime
        {
            get { return _pickupDateTime; }
            set { _pickupDateTime = value; }
        }

        private uint _price;

        public uint Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private uint _userId;

        public uint UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
    }
}
