using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.BusinessLogic;
using RepairShopBusinessLogic.ViewModels;

namespace RepairShopRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly OrderLogic _order;
        private readonly RepairLogic _repair;
        private readonly OrderLogic _main;
        public MainController(OrderLogic order, RepairLogic repair, OrderLogic main)
        {
            _order = order;
            _repair = repair;
            _main = main;
        }
        [HttpGet]
        public List<RepairViewModel> GetRepairList() => _repair.Read(null)?.ToList();
        [HttpGet]
        public RepairViewModel GetRepair(int repairId) => _repair.Read(new RepairBindingModel { Id = repairId })?[0];
        [HttpGet]
        public List<OrderViewModel> GetOrders(int clientId) => _order.Read(new OrderBindingModel { ClientId = clientId });
        [HttpPost]
        public void CreateOrder(CreateOrderBindingModel model) => _main.CreateOrder(model);
    }
}
