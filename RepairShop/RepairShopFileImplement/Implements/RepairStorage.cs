using System;
using System.Collections.Generic;
using System.Linq;
using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.Interfaces;
using RepairShopBusinessLogic.ViewModels;
using RepairShopFileImplement.Models;

namespace RepairShopFileImplement.Implements
{
    public class RepairStorage : IRepairStorage
    {
        private readonly FileDataListSingleton source;

        public RepairStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }

        public List<RepairViewModel> GetFullList()
        {
            return source.Repairs.Select(CreateModel).ToList();
        }

        public List<RepairViewModel> GetFilteredList(RepairBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.Repairs.Where(rec => rec.RepairName.Contains(model.RepairName))
            .Select(CreateModel).ToList();
        }

        public RepairViewModel GetElement(RepairBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var repair = source.Repairs
            .FirstOrDefault(rec => rec.RepairName == model.RepairName || rec.Id == model.Id);
            return repair != null ? CreateModel(repair) : null;
        }

        public void Insert(RepairBindingModel model)
        {
            int maxId = source.Repairs.Count > 0 ? source.Components.Max(rec => rec.Id) : 0;
            var element = new Repair { Id = maxId + 1, RepairComponents = new Dictionary<int, int>() };
            source.Repairs.Add(CreateModel(model, element));
        }

        public void Update(RepairBindingModel model)
        {
            var element = source.Repairs.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, element);
        }

        public void Delete(RepairBindingModel model)
        {
            Repair element = source.Repairs.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                source.Repairs.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
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
            return new RepairViewModel
            {
                Id = repair.Id,
                RepairName = repair.RepairName,
                Price = repair.Price,
                RepairComponents = repair.RepairComponents.ToDictionary(recPC => recPC.Key, recPC =>
                    (source.Components.FirstOrDefault(recC => recC.Id == recPC.Key)?.ComponentName, recPC.Value))
            };
        }
    }
}
