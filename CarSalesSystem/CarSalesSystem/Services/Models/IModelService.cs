using System.Collections.Generic;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services.Models
{
    public interface IModelService
    {
        ICollection<Model> GetAllModels(string Id);
    }
}
