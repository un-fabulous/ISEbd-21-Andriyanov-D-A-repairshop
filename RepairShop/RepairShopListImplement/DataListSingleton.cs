﻿using System;
using System.Collections.Generic;
using System.Text;
using RepairShopListImplement.Models;

namespace RepairShopListImplement
{
    public class DataListSingleton
    {
        private static DataListSingleton instance;
        public List<Component> Components { get; set; }
        public List<Order> Orders { get; set; }
        public List<Repair> Repairs { get; set; }
        public List<Client> Clients { get; set; }
        public List<Warehouse> Warehouses { get; set; }
        private DataListSingleton()
        {
            Components = new List<Component>();
            Orders = new List<Order>();
            Repairs = new List<Repair>();
            Clients = new List<Client>();
            Warehouses = new List<Warehouse>();
        }
        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }
            return instance;
        }
    }
}
