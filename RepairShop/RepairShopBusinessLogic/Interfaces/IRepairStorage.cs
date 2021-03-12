using System;
using System.Collections.Generic;
using System.Text;
using RepairShopBusinessLogic.BindingModels;
using RepairShopBusinessLogic.ViewModels;

namespace RepairShopBusinessLogic.Interfaces
{
    public interface IRepairStorage
    {
        List<RepairViewModel> GetFullList();

        List<RepairViewModel> GetFilteredList(RepairBindingModel model);

        RepairViewModel GetElement(RepairBindingModel model);

        void Insert(RepairBindingModel model);

        void Update(RepairBindingModel model);

        void Delete(RepairBindingModel model);
    }
}
