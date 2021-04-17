using System;
using System.Collections.Generic;
using System.Text;
using RepairShopBusinessLogic.ViewModels;

namespace RepairShopBusinessLogic.HelperModels
{
    class ExcelInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<ReportRepairComponentViewModel> ComponentRepairs { get; set; }
    }
}
