using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APTEventAssignment.Models;
using APTEventAssignment.ViewModels;

namespace APTEventAssignment.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VenuesController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        // GET: Venues
        public ActionResult Index()
        {
            var viewmodel = (from v in db.Venue
                             join vt in db.VenueType on v.Venue_VenueTypeID equals vt.VenueType_ID
                             select new VenueViewModel()
                             {
                                 Venue_ID = v.Venue_ID,
                                 Venue_Name = v.Venue_Name,
                                 Venue_Address = v.Venue_Address,
                                 Venue_Capacity = v.Venue_Capacity,
                                 VenueType_Name = vt.VenueType_Name,
                             });

            return View(viewmodel.ToList());          
        }

        // GET: Venues/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venue venue = db.Venue.Find(id);
            if (venue == null)
            {
                return HttpNotFound();
            }
            //return View(venue);

            var viewmodel = new VenueViewModel
            {
                Venue_ID = venue.Venue_ID,
                Venue_Name = venue.Venue_Name,
                Venue_Address = venue.Venue_Address,
                Venue_Capacity = venue.Venue_Capacity,
                VenueType_Name = venue.VenueType.VenueType_Name,
            };

            //ViewBag.Venue_VenueTypeID = new SelectList(db.VenueType, "VenueType_ID", "VenueType_Name", venue.Venue_VenueTypeID);
            return View(viewmodel);
        }

        // used by the create and edit post methods to map the viewmodel to the model
        private void UpdateVenue (Venue venue, AddVenueViewModel addviewmodel)
        {
            venue.Venue_ID = addviewmodel.Venue_ID;
            venue.Venue_Name = addviewmodel.Venue_Name;
            venue.Venue_Address = addviewmodel.Venue_Address;
            venue.Venue_Capacity = addviewmodel.Venue_Capacity;
            venue.Venue_VenueTypeID = addviewmodel.Venue_VenueTypeID;
        }

        // GET: Venues/Create
        public ActionResult Create()
        {
            ViewBag.Venue_VenueTypeID = new SelectList(db.VenueType, "VenueType_ID", "VenueType_Name");
            return View(new AddVenueViewModel());
            
            //return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddVenueViewModel addviewmodel)
        {
            if (ModelState.IsValid)
            {
                
                var venue = new Venue();

                UpdateVenue(venue, addviewmodel);

                db.Venue.Add(venue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Venue_VenueTypeID = new SelectList(db.VenueType, "VenueType_ID", "VenueType_Name", addviewmodel.Venue_VenueTypeID);
            return View(addviewmodel);
        }


        // GET: Venues/Edit/5
        public ActionResult Edit(int? id)
        {
                        
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Venue venue = db.Venue.Find(id);

            if (venue == null)
            {
                return HttpNotFound();
            }

            var addviewmodel = new AddVenueViewModel
            {
                Venue_ID = venue.Venue_ID,
                Venue_Name = venue.Venue_Name,
                Venue_Address = venue.Venue_Address,
                Venue_Capacity = venue.Venue_Capacity,
                Venue_VenueTypeID = venue.Venue_VenueTypeID,
            };

            ViewBag.Venue_VenueTypeID = new SelectList(db.VenueType, "VenueType_ID", "VenueType_Name", venue.Venue_VenueTypeID);
            return View(addviewmodel);
            //return View(venue);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AddVenueViewModel addviewmodel)
        {

            if (ModelState.IsValid)
            {
                var existingVenue = db.Venue.Find(addviewmodel.Venue_ID);
                UpdateVenue(existingVenue, addviewmodel);

                //db.Entry(addviewmodel).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Venue_VenueTypeID = new SelectList(db.VenueType, "VenueType_ID", "VenueType_Name", addviewmodel.Venue_VenueTypeID);
            return View(addviewmodel);
        }

        // GET: Venues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Venue venue = db.Venue.Find(id);

            if (venue == null)
            {
                return HttpNotFound();
            }

            var viewmodel = new VenueViewModel
            {
                Venue_ID = venue.Venue_ID,
                Venue_Name = venue.Venue_Name,
                Venue_Address = venue.Venue_Address,
                Venue_Capacity = venue.Venue_Capacity,
                VenueType_Name = venue.VenueType.VenueType_Name,
            };

            return View(viewmodel);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Venue venue = db.Venue.Find(id);
            db.Venue.Remove(venue);    
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
    }
}
