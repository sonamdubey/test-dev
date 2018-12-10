using System;

namespace Carwale.UI.ViewModels.Used.Reports
{
    public class CarRegistrationReportViewModel
    {
        public string StateInitials { get; set; }
        public string DistrictSequentialNumber { get; set; }
        public string UniqueIdentifierPrefix { get; set; }
        public string UniqueIdentifierSuffix { get; set; }
        public string RegistrationNumber { get { return $"{StateInitials}{DistrictSequentialNumber}{UniqueIdentifierPrefix}{UniqueIdentifierSuffix}"; } }
    }
    
}