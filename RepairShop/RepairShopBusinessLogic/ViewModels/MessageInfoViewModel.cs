﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RepairShopBusinessLogic.ViewModels
{
    [DataContract]
    public class MessageInfoViewModel
    {
        [DataMember]
        public string MessageId { get; set; }

        [DisplayName("Отправитель")]
        [DataMember]
        public string SenderName { get; set; }

        [DisplayName("Дата письма")]
        [DataMember]
        public DateTime DateDelivery { get; set; }

        [DisplayName("Заголовок")]
        [DataMember]
        public string Subject { get; set; }

        [DisplayName("Текст")]
        [DataMember]
        public string Body { get; set; }
    }
}
