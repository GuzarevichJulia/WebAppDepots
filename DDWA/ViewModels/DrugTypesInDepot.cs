using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDWA.ViewModels
{
    public class DrugTypesInDepot
    {
        public int DepotId { get; set; }
        public List<QuantityDrugType> AvailableDrugTypes { get; set; }
    }
}