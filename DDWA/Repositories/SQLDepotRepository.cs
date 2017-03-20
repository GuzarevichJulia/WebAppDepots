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
    public class SQLDepotRepository : SQLBaseRepository<Depot>, IDepotRepository
    {    
        public SQLDepotRepository(DrugsContext context) : base (context)
        {
            entity = db.Depot;
        }        

        public Depot GetById(int id)
        {
            return entity.Find(id);
        }        

        public void Delete(int id)
        {
            Depot depot = entity.Find(id);
            if(depot != null)
            {
                entity.Remove(depot);
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

        public IQueryable<DepotDrugUnitView> GetDepotsDrugUnits()
        {
            return from depot in entity
                   join drugUnit in db.DrugUnit
                   on depot.DepotId equals drugUnit.DepotId into Joined
                   from drugUnit in Joined.DefaultIfEmpty()
                   select new DepotDrugUnitView { Depot = depot, DrugUnit = drugUnit != null ? drugUnit : null };
        }

    }
}