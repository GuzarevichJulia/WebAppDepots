using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDWA.ViewModels
{
    public class SupplyCountryView
    {
        public int SupplyCountryId { get; set; }

        public int CountryId { get; set; }

        public int DepotId { get; set; }

        public virtual CountryView Country { get; set; }

        public virtual DepotView Depot { get; set; }
    }
}