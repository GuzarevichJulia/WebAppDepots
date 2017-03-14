namespace DDWA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SupplyCountry")]
    public partial class SupplyCountry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SupplyCountryId { get; set; }

        public int CountryId { get; set; }

        public int DepotId { get; set; }

        public virtual Country Country { get; set; }

        public virtual Depot Depot { get; set; }
    }
}
