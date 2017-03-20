using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDWA.Models;

namespace DDWA.ViewModels
{
    public class SendingDrugUnitsModel
    {
        public int DepotId { get; set; }
        public List<DrugUnit> UnshippedDrugUnits { get; set; }
    }
}