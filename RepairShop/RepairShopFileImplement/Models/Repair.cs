using System;
using System.Collections.Generic;
using System.Text;

namespace RepairShopFileImplement.Models
{
    public class Repair
    {
        public int Id { get; set; }

        public string RepairName { get; set; }

        public decimal Price { get; set; }

        public Dictionary<int, int> RepairComponents { get; set; }
    }
}
