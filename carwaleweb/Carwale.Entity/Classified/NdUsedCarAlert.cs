using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified
{
    public class NdUsedCarAlert
    {
        public int UsedCarAlert_Id { get; set; }
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public int CityId { get; set; }
        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public string FuelTypeId { get; set; }
        public string BodyStyleId { get; set; }
        public string TransmissionId { get; set; }
        public string SellerId { get; set; }
        public float? MinBudget { get; set; }
        public float? MaxBudget { get; set; }
        public int MinKms { get; set; }
        public int MaxKms { get; set; }
        public int MinCarAge { get; set; }
        public int MaxCarAge { get; set; }
        public bool NeedOnlyCertifiedCars { get; set; }
        public bool NeedCarWithPhotos { get; set; }
        public string OwnerTypeId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsActive { get; set; }
        public int AlertFrequency { get; set; }
        public string AlertUrl { get; set; }
    }
}
