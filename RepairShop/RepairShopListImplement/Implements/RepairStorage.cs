using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.Interfaces;
using RepairShopBusinessLogic.ViewModels;
using RepairShopListImplement.Models;

namespace RepairShopListImplement.Implements
{
    public class RepairStorage : IRepairStorage
    {
        private readonly DataListSingleton source;
        public RepairStorage()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<RepairViewModel> GetFullList()
        {
            List<RepairViewModel> result = new List<RepairViewModel>();
            foreach (var component in source.Repairs)
            {
                result.Add(CreateModel(component));
            }
            return result;
        }

        public List<RepairViewModel> GetFilteredList(RepairBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            List<RepairViewModel> result = new List<RepairViewModel>();
            foreach (var repair in source.Repairs)
            {
                if (repair.RepairName.Contains(model.RepairName))
                {
                    result.Add(CreateModel(repair));
                }
            }
            return result;
        }
        public RepairViewModel GetElement(RepairBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            foreach (var repair in source.Repairs)
            {
                if (repair.Id == model.Id || repair.RepairName == model.RepairName)
                {
                    return CreateModel(repair);
                }
            }
            return null;
        }
        public void Insert(RepairBindingModel model)
        {
            Repair tempRepair = new Repair { Id = 1, RepairComponents = new Dictionary<int, int>() };
            foreach (var repair in source.Repairs)
            {
                if (repair.Id >= tempRepair.Id)
                {
                    tempRepair.Id = repair.Id + 1;
                }
            }
            source.Repairs.Add(CreateModel(model, tempRepair));
        }
        public void Update(RepairBindingModel model)
        {
            Repair tempRepair = null;
            foreach (var repair in source.Repairs)
            {
                if (repair.Id == model.Id)
                {
                    tempRepair = repair;
                }
            }
            if (tempRepair == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, tempRepair);
        }
        public void Delete(RepairBindingModel model)
        {
            for (int i = 0; i < source.Repairs.Count; ++i)
            {
                if (source.Repairs[i].Id == model.Id)
                {
                    source.Repairs.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
        private Repair CreateModel(RepairBindingModel model, Repair repair)
        {
            repair.RepairName = model.RepairName;
            repair.Price = model.Price;
            // удаляем убранные
            foreach (var key in repair.RepairComponents.Keys.ToList())
            {
                if (!model.RepairComponents.ContainsKey(key))
                {
                    repair.RepairComponents.Remove(key);
                }
            }
            // обновляем существуюущие и добавляем новые
            foreach (var component in model.RepairComponents)
            {
                if (repair.RepairComponents.ContainsKey(component.Key))
                {
                    repair.RepairComponents[component.Key] = model.RepairComponents[component.Key].Item2;
                }
                else
                {
                    repair.RepairComponents.Add(component.Key, model.RepairComponents[component.Key].Item2);
                }
            }
            return repair;
        }
        private RepairViewModel CreateModel(Repair repair)
        {
            // требуется дополнительно получить список компонентов для изделия с названиями и их количество
            Dictionary<int, (string, int)> repairComponents = new Dictionary<int, (string, int)>();
            foreach (var pc in repair.RepairComponents)
            {
                string componentName = string.Empty;
                foreach (var component in source.Components)
                {
                    if (pc.Key == component.Id)
                    {
                        componentName = component.ComponentName;
                        break;
                    }
                }
                repairComponents.Add(pc.Key, (componentName, pc.Value));
            }
            return new RepairViewModel
            {
                Id = repair.Id,
                RepairName = repair.RepairName,
                Price = repair.Price,
                RepairComponents = repairComponents
            };
        }
    }
}
