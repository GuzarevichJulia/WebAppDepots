using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDWA.Interfaces;
using DDWA.Models;
using System.Data.Entity;

namespace DDWA.Repositories
{
    public class SQLDrugUnitRepository : IRepository<DrugUnit>
    {
        private DrugsContext db;

        public SQLDrugUnitRepository(DrugsContext context)
        {
            this.db = context;
        }

        public IQueryable<DrugUnit> GetAll()
        {
            return db.DrugUnit.Include(x => x.Depot);
        }

        public DrugUnit GetById(int id)
        {
            return db.DrugUnit.Find(id);
        }

        public DrugUnit GetById(string id)
        {
            return db.DrugUnit.Find(id);
        }

        public void Create(DrugUnit drugUnit)
        {
            db.DrugUnit.Add(drugUnit);
        }

        public void Update(DrugUnit drugUnit)
        {
            db.Entry(drugUnit).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            DrugUnit drugUnit = db.DrugUnit.Find(id);
            if(drugUnit != null)
            {
                db.DrugUnit.Remove(drugUnit);
            }
        }
    }
}