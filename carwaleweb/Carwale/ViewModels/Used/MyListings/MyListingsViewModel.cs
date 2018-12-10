using Carwale.DTOs.Classified.MyListings;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;

namespace Carwale.UI.ViewModels.Used.MyListings
{
    public class MyListingsViewModel
    {
        public List<MyListingsDTO> Listings { get; set; }
        public Platform Source { get; set; }
    }
}