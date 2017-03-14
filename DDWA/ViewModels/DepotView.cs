using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDWA.ViewModels
{
    public class DepotView
    {
        public int DepotId { get; set; }
        
        public string DepotName { get; set; }

        public int? CountryId { get; set; }

        public virtual CountryView Country { get; set; }

        public virtual ICollection<DrugUnitView> DrugUnit { get; set; }

        public virtual ICollection<SupplyCountryView> SupplyCountry { get; set; }
    }
}