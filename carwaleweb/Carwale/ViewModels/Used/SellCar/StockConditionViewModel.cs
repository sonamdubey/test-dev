using Carwale.Entity.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carwale.UI.ViewModels.Used.SellCar
{
    public class StockConditionViewModel
    {
        public List<StockConditionItems> ListParts { get; set; }
        public List<StockConditionItems> ListMinorScratches { get; set; }
        public List<StockConditionItems> ListDents { get; set; }
        public List<StockConditionItems> ListEngineIssues { get; set; }
        public List<StockConditionItems> ListElectricalIssues { get; set; }
    }
}