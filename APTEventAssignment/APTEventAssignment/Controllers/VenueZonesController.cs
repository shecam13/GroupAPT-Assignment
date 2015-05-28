using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APTEventAssignment.Models;
using APTEventAssignment.ViewModels;

namespace APTEventAssignment.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VenueZonesController : Controller
    {
        private APTEventsEntities db = new APTEventsEntities();

        // GET: VenueZones
        public ActionResult Index()
        {
            var viewmodel = (from vz in db.VenueZone
                             select new VenueZoneViewModel()
                             {
                                 VenueZone_ID = vz.VenueZone_ID,
                                 VenueZone_VenueID = vz.VenueZone_VenueID,
                                 VenueZone_VenueName = vz.Venue.Venue_Name,
                                 VenueZone_Name = vz.VenueZone_Name,
                                 VenueZone_Code = vz.VenueZone_Code,
                                 VenueZone_Columns = vz.VenueZone_Columns,
                                 VenueZone_Rows = vz.VenueZone_Rows,
                             });

            return View(viewmodel.ToList());  
        }

        // GET: VenueZones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VenueZone venueZone = db.VenueZone.Find(id);
            if (venueZone == null)
            {
                return HttpNotFound();
            }

            var viewmodel = new VenueZoneViewModel
            {
                VenueZone_ID = venueZone.VenueZone_ID,
                VenueZone_Name = venueZone.VenueZone_Name,
                VenueZone_VenueID = venueZone.VenueZone_VenueID,
                VenueZone_VenueName = venueZone.Venue.Venue_Name,
                VenueZone_Code = venueZone.VenueZone_Code,
                VenueZone_Columns = venueZone.VenueZone_Columns,
                VenueZone_Rows = venueZone.VenueZone_Rows,
            };

            return View(viewmodel);

        }

        // used by the create and edit post methods to map the viewmodel to the model
        private void UpdateZone(VenueZone venueZone, AddVenueZoneViewModel addviewmodel)
        {
            venueZone.VenueZone_ID = addviewmodel.VenueZone_ID;
            venueZone.VenueZone_VenueID = addviewmodel.VenueZone_VenueID;
            venueZone.VenueZone_Name = addviewmodel.VenueZone_Name;
            venueZone.VenueZone_Code = addviewmodel.VenueZone_Code;
            venueZone.VenueZone_Columns = addviewmodel.VenueZone_Columns;
            venueZone.VenueZone_Rows = addviewmodel.VenueZone_Rows;
        }

        // GET: VenueZones/Create
        public ActionResult Create()
        {
            ViewBag.VenueZone_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name");
            return View(new AddVenueZoneViewModel());
        }

        // POST: VenueZones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddVenueZoneViewModel addviewmodel)
        {
            if (ModelState.IsValid)
            {
                var venueZone = new VenueZone();

                UpdateZone(venueZone, addviewmodel);

                db.VenueZone.Add(venueZone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VenueZone_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", addviewmodel.VenueZone_VenueID);
            return View(addviewmodel);
        }

        // GET: VenueZones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VenueZone venueZone = db.VenueZone.Find(id); if (venueZone == null)
            {
                return HttpNotFound();
            }

            var addviewmodel = new AddVenueZoneViewModel
            {
                VenueZone_ID = venueZone.VenueZone_ID,
                VenueZone_Name = venueZone.VenueZone_Name,
                VenueZone_VenueID = venueZone.VenueZone_VenueID,
                VenueZone_Code = venueZone.VenueZone_Code,
                VenueZone_Columns = venueZone.VenueZone_Columns,
                VenueZone_Rows = venueZone.VenueZone_Rows,
            };

            ViewBag.VenueZone_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", venueZone.VenueZone_VenueID);
            return View(addviewmodel);
        }

        // POST: VenueZones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AddVenueZoneViewModel addviewmodel)
        {
            if (ModelState.IsValid)
            {
                var existingVenueZone = db.VenueZone.Find(addviewmodel.VenueZone_ID);
                UpdateZone(existingVenueZone, addviewmodel);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VenueZone_VenueID = new SelectList(db.Venue, "Venue_ID", "Venue_Name", addviewmodel.VenueZone_VenueID);
            return View(addviewmodel);
        }

        // GET: VenueZones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VenueZone venueZone = db.VenueZone.Find(id); if (venueZone == null)
            {
                return HttpNotFound();
            }

            var viewmodel = new VenueZoneViewModel
            {
                VenueZone_ID = venueZone.VenueZone_ID,
                VenueZone_VenueID = venueZone.VenueZone_VenueID,
                VenueZone_VenueName = venueZone.Venue.Venue_Name,
                VenueZone_Code = venueZone.VenueZone_Code,
                VenueZone_Name = venueZone.VenueZone_Name,
                VenueZone_Columns = venueZone.VenueZone_Columns,
                VenueZone_Rows = venueZone.VenueZone_Rows,
            };

            return View(viewmodel);
        }

        // POST: VenueZones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VenueZone venueZone = db.VenueZone.Find(id);
            db.VenueZone.Remove(venueZone);
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
