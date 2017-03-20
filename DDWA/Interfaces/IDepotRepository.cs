using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDWA.Models;

namespace DDWA.Interfaces
{
    public interface IDepotRepository 
    {
        IQueryable<Depot> GetAll();
        Depot GetById(int id);
        void Create(Depot item);
        void Update(Depot item);
        void Delete(int id);
    }
}
