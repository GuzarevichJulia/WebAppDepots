using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDWA.Models;

namespace DDWA.ViewModels
{
    public class QuantityDrugType
    {
        public int DrugTypeId { get; set; }
        public string DrugTypeName { get; set; }
        public int Quantity { get; set; }

    }
}