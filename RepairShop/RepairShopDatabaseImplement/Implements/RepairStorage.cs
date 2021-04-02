using System;
using System.Collections.Generic;
using System.Linq;
using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.Interfaces;
using RepairShopBusinessLogic.ViewModels;
using RepairShopDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace RepairShopDatabaseImplement.Implements
{
    public class RepairStorage : IRepairStorage
    {
        public List<RepairViewModel> GetFullList()
        {
            using (var context = new RepairShopDatabase())
            {
                return context.Repairs
                .Include(rec => rec.RepairComponents)
                .ThenInclude(rec => rec.Component)
                .ToList()
                .Select(rec => new RepairViewModel
                {
                    Id = rec.Id,
                    RepairName = rec.RepairName,
                    Price = rec.Price,
                    RepairComponents = rec.RepairComponents
                .ToDictionary(recPC => recPC.ComponentId, recPC => (recPC.Component?.ComponentName, recPC.Count))
                })
                .ToList();
            }
        }

        public List<RepairViewModel> GetFilteredList(RepairBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new RepairShopDatabase())
            {
                return context.Repairs
                .Include(rec => rec.RepairComponents)
                .ThenInclude(rec => rec.Component)
                .Where(rec => rec.RepairName.Contains(model.RepairName))
                .ToList()
                .Select(rec => new RepairViewModel
                {
                    Id = rec.Id,
                    RepairName = rec.RepairName,
                    Price = rec.Price,
                    RepairComponents = rec.RepairComponents
                .ToDictionary(recPC => recPC.ComponentId, recPC => (recPC.Component?.ComponentName, recPC.Count))
                }).ToList();
            }
        }

        public RepairViewModel GetElement(RepairBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new RepairShopDatabase())
            {
                var product = context.Repairs
                .Include(rec => rec.RepairComponents)
                .ThenInclude(rec => rec.Component)
                .FirstOrDefault(rec => rec.RepairName == model.RepairName || rec.Id == model.Id);
                return product != null ?
                new RepairViewModel
                {
                    Id = product.Id,
                    RepairName = product.RepairName,
                    Price = product.Price,
                    RepairComponents = product.RepairComponents
                .ToDictionary(recPC => recPC.ComponentId, recPC => (recPC.Component?.ComponentName, recPC.Count))
                } :
                null;
            }
        }

        public void Insert(RepairBindingModel model)
        {
            using (var context = new RepairShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Repair repair = CreateModel(model, new Repair());
                        context.Repairs.Add(repair);                       
                        context.SaveChanges();
                        CreateModel(model, repair, context);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(RepairBindingModel model)
        {
            using (var context = new RepairShopDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Repairs.FirstOrDefault(rec => rec.Id == model.Id);
                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }
                        CreateModel(model, element, context);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(RepairBindingModel model)
        {
            using (var context = new RepairShopDatabase())
            {
                Repair element = context.Repairs.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Repairs.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Repair CreateModel(RepairBindingModel model, Repair repair) 
        {
            repair.RepairName = model.RepairName;
            repair.Price = model.Price;
            return repair;
        }

        private Repair CreateModel(RepairBindingModel model, Repair repair, RepairShopDatabase context)
        {
            repair.RepairName = model.RepairName;
            repair.Price = model.Price;
            if (model.Id.HasValue)
            {
                var productComponents = context.RepairComponents.Where(rec => rec.RepairId == model.Id.Value).ToList();
                // удалили те, которых нет в модели
                context.RepairComponents.RemoveRange(productComponents.Where(rec => !model.RepairComponents.ContainsKey(rec.ComponentId)).ToList());
                context.SaveChanges();
                // обновили количество у существующих записей
                foreach (var updateComponent in productComponents)
                {
                    updateComponent.Count = model.RepairComponents[updateComponent.ComponentId].Item2;
                    model.RepairComponents.Remove(updateComponent.ComponentId);
                }
                context.SaveChanges();
            }
            // добавили новые
            foreach (var pc in model.RepairComponents)
            {
                context.RepairComponents.Add(new RepairComponent
                {
                    RepairId = repair.Id,
                    ComponentId = pc.Key,
                    Count = pc.Value.Item2
                });
                context.SaveChanges();
            }
            return repair;
        }
    }
}
