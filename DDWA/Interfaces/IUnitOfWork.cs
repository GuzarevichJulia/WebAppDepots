using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDWA.Models;

namespace DDWA.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<DrugUnit> DrugUnits { get; }
        IRepository<Depot> Depots { get; }
        void Save();
    }
}
