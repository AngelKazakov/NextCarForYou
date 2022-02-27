using System.Collections.Generic;
using System.Threading.Tasks;
using CarSalesSystem.Data.Models;

namespace CarSalesSystem.Services.Models
{
    public interface IModelService
    {
        ICollection<Model> GetAllModels(string brandId);

       Task< ICollection<Model>> GetAllModelsAsync(string brandId);

    }
}
