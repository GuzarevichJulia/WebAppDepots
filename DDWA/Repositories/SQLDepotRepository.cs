using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDWA.Interfaces;
using DDWA.Models;
using System.Data.Entity;
using DDWA.ViewModels;

namespace DDWA.Repositories
{
    public class SQLDepotRepository : IRepository<Depot>
    {
        private DrugsContext db;

        public SQLDepotRepository(DrugsContext context)
        {
            this.db = context;
        }

        public IQueryable<Depot> GetAll()
        {
            return db.Depot;
        }

        public Depot GetById(int id)
        {
            return db.Depot.Find(id);
        }

        public Depot GetById(string id)
        {
            return db.Depot.Find(id);
        }

        public void Create(Depot drugUnit)
        {
            db.Depot.Add(drugUnit);
        }

        public void Update(Depot depot)
        {
            db.Entry(depot).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Depot depot = db.Depot.Find(id);
            if(depot != null)
            {
                db.Depot.Remove(depot);
            }
        }

        public IQueryable<WeightView> GetTypesWeights()
        {
            return from du in db.DrugUnit
                    where du.DepotId != null
                    group new { du.Depot, du.DrugType, du } by new
                    {
                        du.Depot.DepotName,
                        du.DrugType.DrugTypeName,
                        du.DrugType.DrugTypeWeight
                    } into g
                    select new WeightView()
                    {
                        DepotName = g.Key.DepotName,
                        DrugTypeName = g.Key.DrugTypeName,
                        DrugTypeWeight = g.Key.DrugTypeWeight,
                        Count = g.Count(p => p.du.DrugUnitId != null)
                    };
        }

    }
}