﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RepairShopBusinessLogic.BindingModels
{
    public class CreateOrderBindingModel
    {
        public int RepairId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }
    }
}
