using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class StockImageStatus
    {
        public int? Id { get; set; }
        public ActionType Action { get; set; }
        public string OriginalImagePath { get; set; }

        public enum ActionType
        {
            Approve = 1,
            Delete = 2
        }
    }
}
