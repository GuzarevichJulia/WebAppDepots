using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDWA.ViewModels
{
    public class Shipment
    {
        public List<string> Shipped { get; set; }
        public Dictionary<string, int> Unshipped { get; set; }
    }
}