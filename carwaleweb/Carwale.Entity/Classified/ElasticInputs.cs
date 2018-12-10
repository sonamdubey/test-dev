using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified
{
    public class ElasticInputs
    {
        /// <summary>
        /// Class for Filters Properties ||A user can select in Used Cars Search Page 
        /// Will be fetching FromUri || Added By Jugal
        /// </summary>
        
        public string city { get; set; }
        public string car { get; set; }
        public string root { get; set; }
        public string filterby { get; set; }
        public string fuel { get; set; }
        public string bodytype { get; set; }
        public string seller { get; set; }
        public string owner { get; set; }
        public string trans { get; set; }
        public string color { get; set; }
        public string year { get; set; }
        public string kms { get; set; }
        public string budget { get; set; }

        public string sc { get; set; }
        public string so { get; set; }
        public string pn { get; set; }
        public string ps { get; set; }
        public bool bestmatch { get; set; }

        //// Copy Of Entity
        ///// <summary>
        ///// 
        ///// </summary>
        //public string[] cities { get; set; }
        //public string city { get; set; }

        //public string[] cars { get; set; }
        //public string car { get; set; }

        //public string[] roots { get; set; }
        //public string root { get; set; }

        //public string certifiedCars = string.Empty;
        //public string carsWithPhotos = string.Empty;

        //public string filterby { get; set; }

        //public string[] fuels;
        //public string fuel { get; set; }

        //public string[] bodytypes { get; set; }
        //public string bodytype { get; set; }

        //public string[] sellers { get; set; }
        //public string seller { get; set; }

        //public string[] owners { get; set; }
        //public string owner { get; set; }

        //public string[] transmissions { get; set; }
        //public string trans { get; set; }

        //public string[] colors { get; set; }
        //public string color { get; set; }

        //public string yearL = string.Empty;
        //public string yearH = string.Empty;

        //public string year { get; set; }

        //public string kmL = string.Empty;
        //public string kmH = string.Empty;

        //public string kms { get; set; }

        //public string budgetL = string.Empty;
        //public string budgetH = string.Empty;

        //public string budget { get; set; }

        //public string sc { get; set; }
        //public string so { get; set; }
        //public string pn { get; set; }
        //public string ps { get; set; }
        //public bool bestmatch { get; set; }
    }
}
