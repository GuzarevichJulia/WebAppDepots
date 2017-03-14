using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDWA.ViewModels
{
    public class DrugUnitView
    {
        public string DrugUnitId { get; set; }

        public int? PickNumber { get; set; }

        public int? DepotId { get; set; }

        public int DrugTypeId { get; set; }

        public virtual DepotView Depot { get; set; }

        public virtual DrugTypeView DrugType { get; set; }
    }
}