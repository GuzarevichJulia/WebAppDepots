namespace DDWA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DrugUnit")]
    public partial class DrugUnit
    {
        [StringLength(50)]
        public string DrugUnitId { get; set; }

        public int? PickNumber { get; set; }

        public int? DepotId { get; set; }

        public int DrugTypeId { get; set; }

        public virtual Depot Depot { get; set; }

        public virtual DrugType DrugType { get; set; }
    }
}
