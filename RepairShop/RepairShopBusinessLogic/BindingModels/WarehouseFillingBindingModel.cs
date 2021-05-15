using System;
using System.Collections.Generic;
using System.Text;

namespace RepairShopBusinessLogic.BindingModels
{
    public class WarehouseFillingBindingModel
    {
        public int WarehouseId { get; set; }

        public int ComponentId { get; set; }

        public int Count { get; set; }
    }
}
