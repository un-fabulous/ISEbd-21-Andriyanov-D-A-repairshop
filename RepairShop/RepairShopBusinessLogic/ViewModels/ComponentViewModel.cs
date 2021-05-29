using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using RepairShopBusinessLogic.Attributes;

namespace RepairShopBusinessLogic.ViewModels
{
    public class ComponentViewModel
    {
        [Column(title: "Номер", width: 100, visible: false)]
        public int Id { get; set; }

        [Column(title: "Название компонента", gridViewAutoSize: GridViewAutoSize.Fill)]
        public string ComponentName { get; set; }
    }
}
