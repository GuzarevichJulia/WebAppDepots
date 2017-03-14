namespace DDWA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DrugType")]
    public partial class DrugType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DrugType()
        {
            DrugUnit = new HashSet<DrugUnit>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DrugTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string DrugTypeName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DrugUnit> DrugUnit { get; set; }

        public double DrugTypeWeight { get; set; }
    }
}
