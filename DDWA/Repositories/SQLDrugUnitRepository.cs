using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDWA.Interfaces;
using DDWA.Models;
using System.Data.Entity;

namespace DDWA.Repositories
{
    public class SQLDrugUnitRepository : SQLBaseRepository<DrugUnit>, IDrugUnitRepository
    {
        public SQLDrugUnitRepository(DrugsContext context) : base (context)
        {
            entity = db.DrugUnit;
        }

        public DrugUnit GetById(string id)
        {
            return entity.Find(id);
        }

        public void Delete(string id)
        {
            DrugUnit drugUnit = entity.Find(id);
            if(drugUnit != null)
            {
                entity.Remove(drugUnit);
            }
        }

        public IQueryable<DrugUnit> GetUnshippedDrugUnitsFromDepot(int depotId)
        {
            return from d in entity
                   where d.DepotId == depotId
                   where d.Shipped == false
                   select d;
        }
    }
}