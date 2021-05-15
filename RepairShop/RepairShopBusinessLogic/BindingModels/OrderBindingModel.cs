﻿using System;
using System.Collections.Generic;
using System.Text;
using RepairShopBusinessLogic.Enums;
using System.Runtime.Serialization;

namespace RepairShopBusinessLogic.BindingModels
{
    [DataContract]
    public class OrderBindingModel
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public int? ClientId { get; set; }

        [DataMember]
        public int RepairId { get; set; }

        [DataMember]
        public int? ImplementerId { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Sum { get; set; }

        [DataMember]
        public OrderStatus Status { get; set; }

        [DataMember]
        public DateTime DateCreate { get; set; }

        [DataMember]
        public DateTime? DateImplement { get; set; }

        [DataMember]
        public DateTime? DateFrom { get; set; }

        [DataMember]
        public DateTime? DateTo { get; set; }

        public bool? FreeOrders { get; set; }
    }
}
