using System;
using System.Collections.Generic;
using System.Text;

namespace RepairShopBusinessLogic.ViewModels
{
    public class ReportRepairComponentViewModel
    {
        public string RepairName { get; set; }
        public int TotalCount { get; set; }
        public List<Tuple<string, int>> Components { get; set; }
    }
}
