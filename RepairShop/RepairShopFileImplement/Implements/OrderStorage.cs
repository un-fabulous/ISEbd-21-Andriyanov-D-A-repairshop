﻿using System;
using System.Collections.Generic;
using System.Linq;
using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.Interfaces;
using RepairShopBusinessLogic.ViewModels;
using RepairShopFileImplement.Models;

namespace RepairShopFileImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        private readonly FileDataListSingleton source;

        public OrderStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }

        public List<OrderViewModel> GetFullList()
        {
            return source.Orders.Select(CreateModel).ToList();
        }

        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.Orders.Where(rec => rec.RepairId.ToString().Contains(model.RepairId.ToString())).Select(CreateModel).ToList();
        }

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var order = source.Orders.FirstOrDefault(rec => rec.RepairId == model.RepairId || rec.Id == model.Id);
            return order != null ? CreateModel(order) : null;
        }

        public void Insert(OrderBindingModel model)
        {
            int maxId = source.Orders.Count > 0 ? source.Orders.Max(rec => rec.Id) : 0;
            var order = new Order { Id = maxId + 1 };
            source.Orders.Add(CreateModel(model, order));
        }

        public void Update(OrderBindingModel model)
        {
            var element = source.Orders.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Заказ не найден");
            }
            CreateModel(model, element);
        }

        public void Delete(OrderBindingModel model)
        {
            Order order = source.Orders.FirstOrDefault(rec => rec.Id == model.Id);
            if (order != null)
            {
                source.Orders.Remove(order);
            }
            else
            {
                throw new Exception("Заказ не найден");
            }
        }

        private Order CreateModel(OrderBindingModel model, Order order)
        {
            order.RepairId = model.RepairId;
            order.Count = model.Count;
            order.Sum = model.Sum;
            order.Status = model.Status;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;
            return order;
        }

        private OrderViewModel CreateModel(Order order)
        {
            string repairName = null;

            foreach (var repair in source.Repairs)
            {
                if (repair.Id == order.RepairId)
                {
                    repairName = repair.RepairName;
                }
            }

            return new OrderViewModel
            {
                Id = order.Id,
                RepairId = order.RepairId,
                Count = order.Count,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Sum = order.Sum,
                Status = order.Status,
                RepairName = source.Repairs.FirstOrDefault(rec => rec.Id == order.RepairId)?.RepairName
            };
        }
    }
}
