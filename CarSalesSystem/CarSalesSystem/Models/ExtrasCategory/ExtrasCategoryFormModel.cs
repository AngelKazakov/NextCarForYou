using System.Collections.Generic;

namespace CarSalesSystem.Models.ExtrasCategory
{
    public class ExtrasCategoryFormModel
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public ICollection<AddExtrasFormModel> Extras { get; init; }
    }
}
