﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace RepairShopBusinessLogic.BindingModels
{
    [DataContract]
    public class ClientBindingModel
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string ClientFIO { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }

    }
}
