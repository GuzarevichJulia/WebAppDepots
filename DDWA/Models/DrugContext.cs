using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace DDWA.Models
{
    public class DrugContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Depot> Depots { get; set; }
    }
}