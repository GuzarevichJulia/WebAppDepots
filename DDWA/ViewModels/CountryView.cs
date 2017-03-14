using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDWA.Models;

namespace DDWA.ViewModels
{
    public class CountryView
    {        
        public int CountryId { get; set; }
        
        public string CountryName { get; set; }

        public virtual ICollection<DepotView> Depot { get; set; }

        public virtual ICollection<SupplyCountryView> SupplyCountry { get; set; }
    }
}