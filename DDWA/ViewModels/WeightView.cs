using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDWA.Models;

namespace DDWA.ViewModels
{
    public class WeightView
    {
        public string DepotName { get; set; }
        public string DrugTypeName { get; set; }
        public double DrugTypeWeight { get; set; }
        public List<DrugUnit> DrugUnits { get; set; }
        public double TotalWeight { get; set; }
    }
}