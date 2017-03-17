using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDWA.Models;
using DDWA.ViewModels;
using DDWA.Interfaces;
using DDWA.Repositories;

namespace DDWA.Controllers
{
    public class DepotsController : Controller
    {
        IUnitOfWork Database { get; set; }

        public DepotsController()
        {
            Database = new EFUnitOfWork();
        }

        public ActionResult Depots()
        {
            List<Depot> depots = new List<Depot>(Database.Depots.GetAll());
            ViewBag.Depots = depots;

            Dictionary<int, List<DrugUnit>> depotDrugDictionary = new Dictionary<int, List<DrugUnit>>();

            foreach (var depot in depots)
            {
                List<DrugUnit> drugUnitList = (from d in Database.DrugUnits.GetAll()
                                               where d.DepotId == depot.DepotId
                                               select d).ToList<DrugUnit>();
                depotDrugDictionary.Add(depot.DepotId, drugUnitList);
            }

            return View(depotDrugDictionary);
        }

        public ActionResult DepotsInfo()
        {
            var viewModel = from depot in Database.Depots.GetAll()
                            join drugUnit in Database.DrugUnits.GetAll()
                            on depot.DepotId equals drugUnit.DepotId into Joined
                            from drugUnit in Joined.DefaultIfEmpty()
                            select new DepotDrugUnitView { Depot = depot, DrugUnit = drugUnit != null ? drugUnit : null };
            return View(viewModel);
        }                    

        public ActionResult WeightInfo()
        {
            var groupedList = Database.Depots.GetTypesWeights().ToList();

            double scale = 2.2;;
            foreach (var item in groupedList)
            {
                item.TotalWeight = item.Count * item.DrugTypeWeight / scale;
            }           

            return View(groupedList);
        }
    }
}