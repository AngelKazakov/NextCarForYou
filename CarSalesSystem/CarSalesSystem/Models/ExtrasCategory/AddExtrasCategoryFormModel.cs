﻿using System.Collections.Generic;

namespace CarSalesSystem.Models.ExtrasCategory
{
    public class AddExtrasCategoryFormModel
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public ICollection<AddExtrasFormModel> Extras { get; init; }
    }
}
