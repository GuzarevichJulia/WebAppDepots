using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDWA.Models;
using DDWA.ViewModels;
using AutoMapper;

namespace DDWA.Controllers
{
    public class HomeController : Controller
    {
        DrugsContext db = new DrugsContext();

        public ActionResult Index()
        {
            List<DrugUnit> drugUnits = new List<DrugUnit>(db.DrugUnit);
            List<DrugUnitViewModel> drugUnitsView = new List<DrugUnitViewModel>();

            Mapper.Initialize(cfg => cfg.CreateMap<DrugUnit, DrugUnitViewModel>()
                            .ForMember("DepotName", opt => opt.MapFrom(src => src.Depot.DepotName))
                            .ForMember("DrugTypeName", opt => opt.MapFrom(src => src.DrugType.DrugTypeName)));
            drugUnitsView = Mapper.Map<List<DrugUnit>,List<DrugUnitViewModel>>(drugUnits);

            return View(drugUnitsView);
        }

        [HttpGet]
        public ActionResult EditDrugUnitDepot(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            DrugUnit drugUnit = db.DrugUnit.Find(id);
            if (drugUnit != null)
            {
                SelectList depots = new SelectList(db.Depot, "DepotId", "DepotName", drugUnit.DrugUnitId);
                ViewBag.Depots = depots;
                ViewBag.DrugUnitId = drugUnit.DrugUnitId;
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditDrugUnitDepot(DrugUnitViewModel drugUnitViewModel)
        {
            DrugUnit drugUnit = (from d in db.DrugUnit
                                 where d.DrugUnitId == drugUnitViewModel.DrugUnitId
                                 select d).First();
            if (drugUnitViewModel.DepotId != 0)
            {
                drugUnit.DepotId = drugUnitViewModel.DepotId;
            }
            else
            {
                drugUnit.DepotId = null;
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Depots()
        {
            List<Depot> depots = new List<Depot>(db.Depot);
            ViewBag.Depots = depots;

            Dictionary<int, List<DrugUnit>> depotDrugDictionary = new Dictionary<int, List<DrugUnit>>();

            foreach (var depot in depots)
            {
                List<DrugUnit> drugUnitList = (from d in db.DrugUnit where d.DepotId == depot.DepotId select d).ToList<DrugUnit>();
                depotDrugDictionary.Add(depot.DepotId, drugUnitList);
            }

            return View(depotDrugDictionary);
        }

        public ActionResult DrugUnitsWithDepots()
        {
            var viewModel = from depot in db.Depot
                            join drugUnit in db.DrugUnit
                            on depot.DepotId equals drugUnit.DepotId into Joined
                            from drugUnit in Joined.DefaultIfEmpty()
                            select new DepotDrugUnitView { Depot = depot, DrugUnit = drugUnit != null ? drugUnit : null };
            return View(viewModel);
        }


        public ActionResult SelectDepot()
        {
            List<Depot> depots = new List<Depot>(db.Depot);

            List<DepotViewModel> depotsList = new List<DepotViewModel>();

            Mapper.Initialize(cfg => cfg.CreateMap<Depot, DepotViewModel>());
            depotsList = Mapper.Map<List<Depot>, List<DepotViewModel>>(depots);

            return View(depotsList);
        }

        [HttpGet]
        public ActionResult PickForSending(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            ViewBag.DepotId = id;
            List<DrugUnit> drugUnits = (from d in db.DrugUnit
                                        where d.DepotId == id
                                        where d.Shipped == false
                                        select d).ToList<DrugUnit>();

            List<DrugUnitViewModel> drugUnitsList = new List<DrugUnitViewModel>();
            Mapper.Initialize(cfg => cfg.CreateMap<DrugUnit, DrugUnitViewModel>()
                            .ForMember("DrugTypeName", opt => opt.MapFrom(src => src.DrugType.DrugTypeName)));
            drugUnitsList = Mapper.Map<List<DrugUnit>, List<DrugUnitViewModel>>(drugUnits);

            HashSet<DrugType> drugTypes = new HashSet<DrugType>();
            foreach(var d in drugUnits)
            {
                if (!drugTypes.Contains(d.DrugType))
                {
                    drugTypes.Add(d.DrugType);
                }
            }

            List<DrugTypeQuantity> drugTypesList = new List<DrugTypeQuantity>();
            foreach (var d in drugTypes)
            {
                DrugTypeQuantity drugTypeItem = new DrugTypeQuantity
                {
                    DrugTypeId = d.DrugTypeId,
                    DrugTypeName = d.DrugTypeName,
                    Quantity = 0
                };
                drugTypesList.Add(drugTypeItem);
            }

            ViewBag.DrugUnits = drugUnitsList;      
            return View(drugTypesList);
        }

        [HttpPost]
        public ActionResult PickForSending(List<DrugTypeQuantity> drugTypesList, int depotId)
        {
            int id = depotId;

            List<DrugUnit> drugUnits = (from d in db.DrugUnit
                                        where d.DepotId == id
                                        where d.Shipped == false
                                        select d).ToList<DrugUnit>();

            List<string> shippedDrugUnitsId = new List<string>();

            foreach (var d in drugTypesList)
            {
                List<DrugUnit> drugUnitWithType = (from t in drugUnits
                                                   where t.DrugTypeId == d.DrugTypeId
                                                   select t).ToList<DrugUnit>();

                for (int i = 0; i < d.Quantity; i++)
                {
                    if (i < drugUnitWithType.Count)
                    {
                        drugUnitWithType[i].Shipped = true;
                        shippedDrugUnitsId.Add(drugUnitWithType[i].DrugUnitId);
                    }
                }
            }
            db.SaveChanges();

            ViewBag.DrugUnitsId = shippedDrugUnitsId;
            return View("DisplaySelectedDrugs");
        }

        public ActionResult Weight()
        {
            List<Depot> depots = db.Depot.ToList<Depot>();
            List<DrugType> drugTypes = db.DrugType.ToList<DrugType>();

            var drugUnitsInDepot = (from depot in db.Depot
                            join drugUnit in db.DrugUnit
                            on depot.DepotId equals drugUnit.DepotId
                            select new DepotDrugUnitView { Depot = depot, DrugUnit = drugUnit }).ToList();

            
            Dictionary<Depot, Dictionary<DrugType, List<DepotDrugUnitView>>> drugUnitByDepotDictionary = new Dictionary<Depot, Dictionary<DrugType, List<DepotDrugUnitView>>>();

            foreach (var depot in depots)
            {
                Dictionary<DrugType, List<DepotDrugUnitView>> drugUnitByTypeDictionary = new Dictionary<DrugType, List<DepotDrugUnitView>>();
                foreach (var type in drugTypes)
                {
                    var drugUnitsByDepot = (from d in drugUnitsInDepot
                                            where d.Depot.DepotId == depot.DepotId
                                            where d.DrugUnit.DrugTypeId == type.DrugTypeId
                                            select d).ToList<DepotDrugUnitView>();
                    if ((drugUnitsByDepot != null) && (drugUnitsByDepot.Count > 0))
                    {
                        drugUnitByTypeDictionary.Add(type, drugUnitsByDepot);
                    }
                }
                drugUnitByDepotDictionary.Add(depot, drugUnitByTypeDictionary);
            }
            
            List<double> weightValues = new List<double>();
            double scale = 2.2;

            foreach (var d in drugUnitByDepotDictionary)
            {
                if(d.Value.Count > 0)
                {
                    foreach(var t in d.Value)
                    {
                        weightValues.Add(t.Value.Count*t.Key.DrugTypeWeight/scale);
                    }
                }
            }

            ViewBag.ValueList = weightValues;
            return View(drugUnitByDepotDictionary);
        }

    }
}