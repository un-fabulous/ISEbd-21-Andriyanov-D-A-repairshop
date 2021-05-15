﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.Interfaces;
using RepairShopBusinessLogic.ViewModels;
using RepairShopBusinessLogic.Enums;

namespace RepairShopBusinessLogic.BusinessLogic
{
    public class WorkModeling
    {
        private readonly IImplementerStorage _implementerStorage;

        private readonly IOrderStorage _orderStorage;

        private readonly OrderLogic _orderLogic;

        private readonly Random rnd;

        public WorkModeling(IImplementerStorage implementerStorage, IOrderStorage orderStorage, OrderLogic orderLogic)
        {
            this._implementerStorage = implementerStorage;
            this._orderStorage = orderStorage;
            this._orderLogic = orderLogic;
            rnd = new Random(1000);
        }

        public void DoWork()
        {
            var implementers = _implementerStorage.GetFullList();
            var orders = _orderStorage.GetFilteredList(new OrderBindingModel { FreeOrders = true });
            foreach (var implementer in implementers)
            {
                WorkerWorkAsync(implementer, orders);
            }
        }

        private async void WorkerWorkAsync(ImplementerViewModel implementer, List<OrderViewModel> orders)
        {
            // ищем заказы, которые уже в работе (вдруг исполнителя прервали)
            var runOrders = await Task.Run(() => _orderStorage.GetFilteredList(new OrderBindingModel { ImplementerId = implementer.Id }));

            var needComponentOrders = await Task.Run(() => _orderStorage.GetFilteredList(new OrderBindingModel { NeedComponentOrders = true, ImplementerId = implementer.Id }));

            foreach (var order in runOrders)
            {
                // делаем работу заново
                Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                _orderLogic.FinishOrder(new ChangeStatusBindingModel { OrderId = order.Id, ImplementerId = implementer.Id });
                // отдыхаем
                Thread.Sleep(implementer.PauseTime);
            }

            foreach (var order in needComponentOrders)
            {
                try
                {
                    _orderLogic.TakeOrderInWork(new ChangeStatusBindingModel
                    {
                        OrderId = order.Id,
                        ImplementerId = implementer.Id
                    });
                    if (_orderStorage.GetElement(new OrderBindingModel { Id = order.Id }).Status == OrderStatus.ТребуютсяМатериалы)
                    {
                        continue;
                    }
                    Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                    _orderLogic.FinishOrder(new ChangeStatusBindingModel
                    {
                        OrderId = order.Id
                    });
                    Thread.Sleep(implementer.PauseTime);
                }
                catch (Exception) { }
            }

            await Task.Run(() =>
            {
                foreach (var order in orders)
                {
                    // пытаемся назначить заказ на исполнителя
                    try
                    {
                        _orderLogic.TakeOrderInWork(new ChangeStatusBindingModel { OrderId = order.Id, ImplementerId = implementer.Id });
                        if (_orderStorage.GetElement(new OrderBindingModel { Id = order.Id }).Status == OrderStatus.ТребуютсяМатериалы)
                        {
                            continue;
                        }
                        // делаем работу
                        Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                        _orderLogic.FinishOrder(new ChangeStatusBindingModel { OrderId = order.Id, ImplementerId = implementer.Id });
                        // отдыхаем
                        Thread.Sleep(implementer.PauseTime);
                    }
                    catch (Exception) { }
                }
            });
        }
    }
}
