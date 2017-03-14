using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDWA.ViewModels
{
    public class DrugTypeView
    {
        public int DrugTypeId { get; set; }

        public string DrugTypeName { get; set; }

        public virtual ICollection<DrugUnitView> DrugUnit { get; set; }
    }
}