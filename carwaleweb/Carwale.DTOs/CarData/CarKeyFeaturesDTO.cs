using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.CarData;
namespace Carwale.DTOs.CarData
{
    public class CarKeyFeaturesDto
    {
       public string MakeName { get; set; }
       public string ModelName { get; set; }
       public string Mileage { get; set; }
       public string Price { get; set; }
       public string Engine { get; set; }
       public string Transmission { get; set; }
    }
}