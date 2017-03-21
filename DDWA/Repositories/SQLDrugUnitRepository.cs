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

        public IQueryable<DrugUnit> GetAvailableDrugUnitsFromDepot(int depotId)
        {
            return from d in entity
                   where d.DepotId == depotId
                   where d.Shipped == false
                   select d;
        }

        public DrugTypesInDepot GetAvailableDrugTypesInDepot(int depotId)
        {
            var availableDrugTypes = (from d in entity
                    where  d.DepotId == depotId
                    where d.Shipped == false
                    select new QuantityDrugType()
                    {
                        DrugTypeName = d.DrugType.DrugTypeName,
                        DrugTypeId = d.DrugTypeId,
                        Quantity = 0
                    }).Distinct().ToList();
            return new DrugTypesInDepot
            {
                DepotId = depotId,
                AvailableDrugTypes = availableDrugTypes
            };
        }

        public Shipment Send(DrugTypesInDepot drugTypesInDepot)
        {
            var drugUnits = (from d in entity
                                        where d.DepotId == drugTypesInDepot.DepotId
                                        where d.Shipped == false
                                        select d).ToList();


            var shippedDrugUnitsId = new List<string>();
            var unshippedDrugUnits = new Dictionary<string, int>();

            foreach (var d in drugTypesInDepot.AvailableDrugTypes)
            {
                var drugUnitWithType = (from t in drugUnits
                                                   where t.DrugTypeId == d.DrugTypeId
                                                   select t).ToList();

                int shippedCount;
                int unshippedCount = 0;
                if (d.Quantity < 0)
                {
                    shippedCount = 0;
                }
                if ((drugUnitWithType.Count - d.Quantity) < 0)
                {
                    shippedCount = drugUnitWithType.Count;
                    unshippedCount = d.Quantity - drugUnitWithType.Count;
                    unshippedDrugUnits.Add(d.DrugTypeName, unshippedCount);
                }
                else
                {
                    shippedCount = d.Quantity;
                }

                for (int i = 0; i < shippedCount; i++)
                {
                    drugUnitWithType[i].Shipped = true;
                    shippedDrugUnitsId.Add(drugUnitWithType[i].DrugUnitId);
                }
            }
            db.SaveChanges();

            return new Shipment
            {
                Shipped = shippedDrugUnitsId,
                Unshipped = unshippedDrugUnits
            };
        }
    }
}