﻿using System;
using System.Collections.Generic;
using System.Text;
using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.Interfaces;
using RepairShopBusinessLogic.ViewModels;
using RepairShopBusinessLogic.Enums;
using RepairShopListImplement.Models;

namespace RepairShopListImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        private readonly DataListSingleton source;

        public OrderStorage()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<OrderViewModel> GetFullList()
        {
            List<OrderViewModel> result = new List<OrderViewModel>();
            foreach (var component in source.Orders)
            {
                result.Add(CreateModel(component));
            }
            return result;
        }

        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            List<OrderViewModel> result = new List<OrderViewModel>();

            foreach (var order in source.Orders)
            {
                if ((!model.DateFrom.HasValue && !model.DateTo.HasValue &&
                    order.DateCreate.Date == model.DateCreate.Date) ||
                    (model.DateFrom.HasValue && model.DateTo.HasValue &&
                    order.DateCreate.Date >= model.DateFrom.Value.Date && order.DateCreate.Date <=
                    model.DateTo.Value.Date) ||
                    (model.ClientId.HasValue && order.ClientId == model.ClientId) ||
                    (model.FreeOrders.HasValue && model.FreeOrders.Value && order.Status == OrderStatus.Принят) ||
                    (model.ImplementerId.HasValue && order.ImplementerId ==
                    model.ImplementerId && order.Status == OrderStatus.Выполняется))
                {
                    result.Add(CreateModel(order));
                }
            }
            return result;
        }

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            foreach (var component in source.Orders)
            {
                if (component.Id == model.Id || component.RepairId == model.RepairId)
                {
                    return CreateModel(component);
                }
            }
            return null;
        }

        public void Insert(OrderBindingModel model)
        {
            Order tempComponent = new Order { Id = 1 };
            foreach (var component in source.Orders)
            {
                if (component.Id >= tempComponent.Id)
                {
                    tempComponent.Id = component.Id + 1;
                }
            }
            source.Orders.Add(CreateModel(model, tempComponent));
        }

        public void Update(OrderBindingModel model)
        {
            Order tempComponent = null;
            foreach (var component in source.Orders)
            {
                if (component.Id == model.Id)
                {
                    tempComponent = component;
                }
            }
            if (tempComponent == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, tempComponent);
        }

        public void Delete(OrderBindingModel model)
        {
            for (int i = 0; i < source.Components.Count; ++i)
            {
                if (source.Components[i].Id == model.Id.Value)
                {
                    source.Components.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }

        private Order CreateModel(OrderBindingModel model, Order order)
        {
            order.ClientId = (int)model.ClientId;
            order.ImplementerId = model.ImplementerId;
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

            string clientFio = null;

            foreach (var client in source.Clients)
            {
                if (client.Id == order.RepairId)
                {
                    clientFio = client.ClientFIO;
                }
            }

            string implementerFio = null;
            foreach (var implementer in source.Implementers)
            {
                if (implementer.Id == order.RepairId)
                {
                    implementerFio = implementer.ImplementerFIO;
                }
            }


            return new OrderViewModel
            {
                Id = order.Id,
                ClientId = order.ClientId,
                RepairId = order.RepairId,
                Count = order.Count,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Sum = order.Sum,
                Status = order.Status,
                RepairName = repairName,
                ClientFIO = clientFio,
                ImplementerFIO = implementerFio
            };
        }
    }
}
