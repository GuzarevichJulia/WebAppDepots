using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDWA.ViewModels
{
    public class EditingDrugUnitModel
    {
        public SelectList DepotsList { get; set; }
        public string DrugUnitId { get; set; }
    }
}