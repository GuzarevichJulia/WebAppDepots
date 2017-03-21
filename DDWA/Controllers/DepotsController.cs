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

        public ActionResult DepotsInfo()
        {
            var depotsDrugUnitsList = Database.Depots.GetDrugUnitsFromDepots().ToList();

            return View(depotsDrugUnitsList);
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