using Carwale.DTOs.NewCars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carwale.UI.ViewModels.NewCars.Make
{
    public class VersionListViewModel
    {
        public string MakeName { get; set; }
        public List<NewCarVersionsDTO> Versions { get; set; }
    }
}