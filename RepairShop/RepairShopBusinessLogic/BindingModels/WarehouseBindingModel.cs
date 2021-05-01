﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RepairShopBusinessLogic.BindingModels
{
    public class WarehouseBindingModel
    {
        public int? Id { get; set; }

        public string WarehouseName { get; set; }

        public string Responsible { get; set; }

        public DateTime DateCreate { get; set; }

        public Dictionary<int, (string, int)> WarehouseComponents { get; set; }
    }
}
