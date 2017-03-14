namespace DDWA.Models
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Depot")]
    public partial class Depot
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Depot()
        {
            DrugUnit = new HashSet<DrugUnit>();
            SupplyCountry = new HashSet<SupplyCountry>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DepotId { get; set; }

        [Required]
        [StringLength(50)]
        public string DepotName { get; set; }

        public int? CountryId { get; set; }

        public virtual Country Country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DrugUnit> DrugUnit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplyCountry> SupplyCountry { get; set; }
    }
}
