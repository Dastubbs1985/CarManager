using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarManager.Models;

namespace CarManager.Controllers
{
    public class CarController : Controller
    {
        private CarManagerEntities db = new CarManagerEntities();

        // GET: Car
        public ActionResult Index(string sortOrder)
        {
            

            ViewBag.MakeSortParam = String.IsNullOrEmpty(sortOrder) ? "carMake_desc" : "";
            ViewBag.ModelSortParam = sortOrder == "Model" ? "carModel_desc" : "Model";
            ViewBag.ColourSortParam = sortOrder == "Colour" ? "carColour_desc" : "Colour";
            ViewBag.EngineSortParam = sortOrder == "Engine" ? "carEngine_desc" : "Engine";

            var cars = from x in db.Cars.Include(c => c.CarModel).Include(c => c.CarMake).Include(c => c.Colour)
                       select x;
            switch (sortOrder)
            {
                case "Model":
                    cars = cars.OrderBy(x => x.C_fk_CarModel);
                    break;
                case "carModel_desc":
                    cars = cars.OrderByDescending(x => x.C_fk_CarModel);
                    break;
                case "Colour":
                    cars = cars.OrderBy(x => x.C_fk_CarColour);
                    break;
                case "carColour_desc":
                    cars = cars.OrderByDescending(x => x.C_fk_CarColour);
                    break;
                case "Engine":
                    cars = cars.OrderBy(x => x.CarEngine);
                    break;
                case "carEngine_desc":
                    cars = cars.OrderByDescending(x => x.CarEngine);
                    break;
                case "carMake_desc":
                    cars = cars.OrderByDescending(x => x.C_fk_CarMake);
                    break;
                default:
                    cars = cars.OrderBy(x => x.C_fk_CarMake);
                    break;
            }

            return View(cars);
        }

        // GET: Car/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Car/Create
        public ActionResult Create()
        {
            ViewBag.C_fk_CarModel = new SelectList(db.CarModels.OrderBy(x => x.ModelName), "ModelID", "ModelName");
            ViewBag.C_fk_CarMake = new SelectList(db.CarMakes.OrderBy(x => x.MakeName), "MakeID", "MakeName");
            ViewBag.C_fk_CarColour = new SelectList(db.Colours.OrderBy(x => x.ColourName), "ColourID", "ColourName");
            return View();
        }

        // POST: Car/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CarID,C_fk_CarModel,C_fk_CarMake,C_fk_CarColour,CarEngine")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.C_fk_CarModel = new SelectList(db.CarModels.OrderBy(x=>x.ModelName), "ModelID", "ModelName", car.C_fk_CarModel);
            ViewBag.C_fk_CarMake = new SelectList(db.CarMakes.OrderBy(x => x.MakeName), "MakeID", "MakeName", car.C_fk_CarMake);
            ViewBag.C_fk_CarColour = new SelectList(db.Colours.OrderBy(x => x.ColourName), "ColourID", "ColourName", car.C_fk_CarColour);
            return View(car);
        }

        // GET: Car/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.C_fk_CarModel = new SelectList(db.CarModels, "ModelID", "ModelName", car.C_fk_CarModel);
            ViewBag.C_fk_CarMake = new SelectList(db.CarMakes, "MakeID", "MakeName", car.C_fk_CarMake);
            ViewBag.C_fk_CarColour = new SelectList(db.Colours, "ColourID", "ColourName", car.C_fk_CarColour);
            return View(car);
        }

        // POST: Car/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CarID,C_fk_CarModel,C_fk_CarMake,C_fk_CarColour,CarEngine")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.C_fk_CarModel = new SelectList(db.CarModels, "ModelID", "ModelName", car.C_fk_CarModel);
            ViewBag.C_fk_CarMake = new SelectList(db.CarMakes, "MakeID", "MakeName", car.C_fk_CarMake);
            ViewBag.C_fk_CarColour = new SelectList(db.Colours, "ColourID", "ColourName", car.C_fk_CarColour);
            return View(car);
        }

        // GET: Car/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Car/AddModel
        public ActionResult AddModel()
        {
            return View();
        }

        // POST: Car/AddModel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddModel([Bind(Include = "ModelID,ModelName")] CarModel carModel)
        {
            if (ModelState.IsValid)
            {
                db.CarModels.Add(carModel);
                db.SaveChanges();
                return RedirectToAction("AddModel");
            }
            return View(carModel);
        }

        // GET: Car/AddMake
        public ActionResult AddMake()
        {
            return View();
        }

        // POST: Car/AddMake
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMake([Bind(Include = "MakeID,MakeName")] CarMake carMake)
        {
            if (ModelState.IsValid)
            {
                db.CarMakes.Add(carMake);
                db.SaveChanges();
                return RedirectToAction("AddMake");
            }
            return View(carMake);
        }

        // GET: Car/AddColour
        public ActionResult AddColour()
        {
            return View();
        }

        // POST: Car/AddColour
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddColour([Bind(Include = "ColourID,ColourName")] Colour carColour)
        {
            if (ModelState.IsValid)
            {
                db.Colours.Add(carColour);
                db.SaveChanges();
                return RedirectToAction("AddColour");
            }
            return View(carColour);
        }
    }
}
