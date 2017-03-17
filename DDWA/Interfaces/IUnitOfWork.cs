using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDWA.Models;
using DDWA.Repositories;

namespace DDWA.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<DrugUnit> DrugUnits { get; }
        SQLDepotRepository Depots { get; }
        void Save();
    }
}
