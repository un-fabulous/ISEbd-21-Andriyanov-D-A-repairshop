using System;
using System.Collections.Generic;
using System.Text;
using RepairShopBusinessLogic.Enums;
using System.ComponentModel;

namespace RepairShopBusinessLogic.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public int RepairId { get; set; }
        [DisplayName("Изделие")]

        public string RepairName { get; set; }
        [DisplayName("Количество")]

        public int Count { get; set; }
        [DisplayName("Сумма")]

        public decimal Sum { get; set; }
        [DisplayName("Статус")]

        public OrderStatus Status { get; set; }
        [DisplayName("Дата создания")]

        public DateTime DateCreate { get; set; }
        [DisplayName("Дата выполнения")]

        public DateTime? DateImplement { get; set; }
    }
}
