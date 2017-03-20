using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDWA.Models;

namespace DDWA.Interfaces
{
    public interface IDrugUnitRepository
    {
        IQueryable<DrugUnit> GetAll();
        DrugUnit GetById(string id);
        void Create(DrugUnit item);
        void Update(DrugUnit item);
        void Delete(string id);
    }
}
