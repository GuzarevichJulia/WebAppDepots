using AutoMapper;
using DDWA.Models;
using DDWA.ViewModels;
using DDWA.Interfaces;
using DDWA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDWA.Controllers
{
    public class DrugUnitsController : Controller
    {
        IUnitOfWork Database { get; set; }

        public DrugUnitsController()
        {
            Database = new EFUnitOfWork();
        }

        public ActionResult Index()
        {
            List<DrugUnit> drugUnits = new List<DrugUnit>(Database.DrugUnits.GetAll());
            List<DrugUnitViewModel> drugUnitsView = new List<DrugUnitViewModel>();

            Mapper.Initialize(cfg => cfg.CreateMap<DrugUnit, DrugUnitViewModel>()
                            .ForMember("DepotName", opt => opt.MapFrom(src => src.Depot.DepotName))
                            .ForMember("DrugTypeName", opt => opt.MapFrom(src => src.DrugType.DrugTypeName)));
            drugUnitsView = Mapper.Map<List<DrugUnit>, List<DrugUnitViewModel>>(drugUnits);

            return View(drugUnitsView);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            DrugUnit drugUnit = Database.DrugUnits.GetById(id);
            if (drugUnit != null)
            {
                SelectList depots = new SelectList(Database.Depots.GetAll(), "DepotId", "DepotName", drugUnit.DrugUnitId);
                ViewBag.Depots = depots;
                ViewBag.DrugUnitId = drugUnit.DrugUnitId;
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(DrugUnitViewModel drugUnitViewModel)
        {
            DrugUnit drugUnit = (from d in Database.DrugUnits.GetAll()
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
            Database.Save();

            return RedirectToAction("Index");
        }

        public ActionResult Select()
        {
            List<Depot> depots = new List<Depot>(Database.Depots.GetAll());

            List<DepotViewModel> depotsList = new List<DepotViewModel>();

            Mapper.Initialize(cfg => cfg.CreateMap<Depot, DepotViewModel>());
            depotsList = Mapper.Map<List<Depot>, List<DepotViewModel>>(depots);

            return View(depotsList);
        }

        [HttpGet]
        public ActionResult Send(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            ViewBag.DepotId = id;
            List<DrugUnit> drugUnits = (from d in Database.DrugUnits.GetAll()
                                        where d.DepotId == id
                                        where d.Shipped == false
                                        select d).ToList<DrugUnit>();

            List<DrugUnitViewModel> drugUnitsList = new List<DrugUnitViewModel>();
            Mapper.Initialize(cfg => cfg.CreateMap<DrugUnit, DrugUnitViewModel>()
                            .ForMember("DrugTypeName", opt => opt.MapFrom(src => src.DrugType.DrugTypeName)));
            drugUnitsList = Mapper.Map<List<DrugUnit>, List<DrugUnitViewModel>>(drugUnits);

            HashSet<DrugType> drugTypes = new HashSet<DrugType>();
            foreach (var d in drugUnits)
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
        public ActionResult Send(List<DrugTypeQuantity> drugTypesList, int depotId)
        {
            int id = depotId;

            List<DrugUnit> drugUnits = (from d in Database.DrugUnits.GetAll()
                                        where d.DepotId == id
                                        where d.Shipped == false
                                        select d).ToList<DrugUnit>();

            List<string> shippedDrugUnitsId = new List<string>();
            Dictionary<string, int> unshippedDrugUnits = new Dictionary<string, int>();

            foreach (var d in drugTypesList)
            {
                List<DrugUnit> drugUnitWithType = (from t in drugUnits
                                                   where t.DrugTypeId == d.DrugTypeId
                                                   select t).ToList<DrugUnit>();

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
            Database.Save();

            ViewBag.DrugUnitsId = shippedDrugUnitsId;
            ViewBag.UnshippedDrugUnits = unshippedDrugUnits;
            return View("Display");
        }

    }
}