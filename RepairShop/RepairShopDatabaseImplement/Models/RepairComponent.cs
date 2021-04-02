using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RepairShopDatabaseImplement.Models
{
    public class RepairComponent
    {
        public int Id { get; set; }

        public int RepairId { get; set; }

        public int ComponentId { get; set; }

        [Required]
        public int Count { get; set; }

        public virtual Component Component { get; set; }

        public virtual Repair Repair { get; set; }
    }
}
