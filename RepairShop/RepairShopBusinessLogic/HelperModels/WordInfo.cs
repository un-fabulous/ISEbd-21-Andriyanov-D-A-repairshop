using System;
using System.Collections.Generic;
using System.Text;
using RepairShopBusinessLogic.ViewModels;

namespace RepairShopBusinessLogic.HelperModels
{
    class WordInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<RepairViewModel> Repairs { get; set; }
    }
}
