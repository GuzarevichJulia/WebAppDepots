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
                EditingDrugUnitModel editingDrugUnit = new EditingDrugUnitModel
                {
                    DepotsList = new SelectList(Database.Depots.GetAll(), "DepotId", "DepotName", drugUnit.DrugUnitId),
                    DrugUnitId = drugUnit.DrugUnitId
                };
                return View(editingDrugUnit);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(EditedDrugUnitModel editedDrugUnit)
        {
            DrugUnit drugUnit = Database.DrugUnits.GetById(editedDrugUnit.DrugUnitId);
            if (editedDrugUnit.DepotId != 0)
            {
                drugUnit.DepotId = editedDrugUnit.DepotId;
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

            List<DepotViewModel> depotsView = new List<DepotViewModel>();
            Mapper.Initialize(cfg => cfg.CreateMap<Depot, DepotViewModel>());
            depotsView = Mapper.Map<List<Depot>, List<DepotViewModel>>(depots);

            return View(depotsView);
        }

        [HttpGet]
        public ActionResult Send(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }          

            var drugTypesInDepot = Database.DrugUnits.GetAvailableDrugTypesInDepot((int)id);

            //not necessary, for clarity when choosing the quantity of a certain type
            var availableDrugUnits = Database.DrugUnits.GetAvailableDrugUnitsFromDepot((int)id).ToList();
            List<DrugUnitViewModel> availableDrugUnitsView = new List<DrugUnitViewModel>();
            Mapper.Initialize(cfg => cfg.CreateMap<DrugUnit, DrugUnitViewModel>()
                            .ForMember("DrugTypeName", opt => opt.MapFrom(src => src.DrugType.DrugTypeName)));
            availableDrugUnitsView = Mapper.Map<List<DrugUnit>, List<DrugUnitViewModel>>(availableDrugUnits);
            ViewBag.DrugUnits = availableDrugUnitsView;

            return View(drugTypesInDepot);
        }

        [HttpPost]
        public ActionResult Send(DrugTypesInDepot drugTypesInDepot)
        {
            Shipment shipment = Database.DrugUnits.Send(drugTypesInDepot);

            return View("Display",shipment);
        }

    }
}